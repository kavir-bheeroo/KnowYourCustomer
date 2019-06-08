using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Parsers;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Options;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Processors
{
    public class AbbyyMrzProcessor : IMrzProcessor
    {
        private readonly MrzProviderOptions _providerOptions;

        public AbbyyMrzProcessor(IOptions<MrzProviderOptions> options)
        {
            _providerOptions = Guard.IsNotNull(options, nameof(options)).Value;
        }

        public MrzSubmitResponse ProcessMrzFile(MrzSubmitRequest request)
        {
            string url = string.Format("{0}/processMRZ", _providerOptions.Url);
            WebRequest webRequest = CreatePostRequest(url);
            WriteFileToRequest(request.FilePath, webRequest);

            XDocument response = PerformRequest(webRequest);
            var abbyyOcrTask = XmlParser.GetTaskStatus(response);
            return new MrzSubmitResponse { TaskId = abbyyOcrTask.TaskId.Id, Status = abbyyOcrTask.Status.ToString() };
        }

        public MrzStatusResponse GetMrzTaskStatus(MrzStatusRequest request)
        {
            // Get Task Status
            string url = string.Format("{0}/getTaskStatus?taskId={1}", _providerOptions.Url, Uri.EscapeDataString(request.TaskId));

            Thread.Sleep(3000);
            WebRequest webRequest = CreateGetRequest(url);
            XDocument response = PerformRequest(webRequest);
            var abbyyOcrTask = XmlParser.GetTaskStatus(response);

            if (abbyyOcrTask.Status != TaskStatus.Completed)
            {
                return new MrzStatusResponse();
            }

            // Download Task response file
            var kycFolderResponsePath = Path.Combine(Environment.CurrentDirectory, _providerOptions.StorageFolder);
            Directory.CreateDirectory(kycFolderResponsePath);
            var path = Path.Combine(kycFolderResponsePath, Path.GetFileNameWithoutExtension(request.KycId.ToString()) + $"_{DateTime.UtcNow.ToString("ddMMyyy:hhmmss")}.xml");
            DownloadResult(abbyyOcrTask, path);

            // Deserialize into response object
            var serializer = new XmlSerializer(typeof(DocumentType));

            using (var reader = new StreamReader(path))
            {
                var deserializedResponse = (DocumentType)serializer.Deserialize(reader);
                var dictionary = deserializedResponse.ToDictionary();
                return new MrzStatusResponse(dictionary);
            }
        }

        private void DownloadResult(OcrTask task, string outputFile)
        {
            if (task.Status != TaskStatus.Completed)
            {
                throw new ArgumentException("Cannot download result for not completed task");
            }

            try
            {
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                if (task.DownloadUrls == null || task.DownloadUrls.Count == 0)
                {
                    throw new ArgumentException("Cannot download task without download url");
                }

                string url = task.DownloadUrls[0];
                DownloadUrl(url, outputFile);
            }
            catch (WebException e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void DownloadUrl(string url, string outputFile)
        {
            try
            {
                WebRequest request = CreateGetRequest(url);

                using (HttpWebResponse result = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = result.GetResponseStream())
                    {
                        // Write result directly to file
                        using (Stream file = File.OpenWrite(outputFile))
                        {
                            CopyStream(stream, file);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private HttpWebRequest CreatePostRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (url.StartsWith(_providerOptions.Url, StringComparison.InvariantCultureIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding(_providerOptions.Encoding);
                string toEncode = $"{_providerOptions.Username}:{_providerOptions.Password}";
                string baseEncoded = Convert.ToBase64String(encoding.GetBytes(toEncode));
                request.Headers.Add("Authorization", "Basic " + baseEncoded);
            }

            request.Method = "POST";
            request.ContentType = "application/octet-stream";
            return request;
        }

        private HttpWebRequest CreateGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (url.StartsWith(_providerOptions.Url, StringComparison.InvariantCultureIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding(_providerOptions.Encoding);
                string toEncode = $"{_providerOptions.Username}:{_providerOptions.Password}";
                string baseEncoded = Convert.ToBase64String(encoding.GetBytes(toEncode));
                request.Headers.Add("Authorization", "Basic " + baseEncoded);
            }

            request.Method = "GET";
            return request;
        }

        private void WriteFileToRequest(string filePath, WebRequest request)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                request.ContentLength = reader.BaseStream.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] buf = new byte[reader.BaseStream.Length];
                    while (true)
                    {
                        int bytesRead = reader.Read(buf, 0, buf.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        stream.Write(buf, 0, bytesRead);
                    }
                }
            }
        }

        private XDocument PerformRequest(WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                {
                    return ParseAsXml((HttpWebResponse)response);
                }
            }
            catch (WebException e)
            {
                throw e;
            }
        }

        private static XDocument ParseAsXml(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (var reader = new XmlTextReader(stream))
                {
                    XDocument responseXml = XDocument.Load(reader);
                    return responseXml;
                }
            }
        }
    }
}