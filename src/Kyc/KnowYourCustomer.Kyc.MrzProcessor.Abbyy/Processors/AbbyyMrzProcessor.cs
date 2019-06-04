using KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Parsers;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Processors
{
    public class AbbyyMrzProcessor : IMrzProcessor
    {
        //private readonly HttpClient _httpClient;

        private const string ServerUrl = "https://cloud-eu.ocrsdk.com/";

        public AbbyyMrzProcessor()//HttpClient httpClient)
        {
            //_httpClient = Guard.IsNotNull(httpClient, nameof(httpClient));
        }

        public MrzProcessResponse ProcessMrzFile(MrzProcessRequest request)
        {
            //_httpClient.BaseAddress = new Uri("https://cloud-eu.ocrsdk.com/");
            //_httpClient.PostAsync()

            //string url = string.Format("{0}/processMRZ", ServerUrl);
            //WebRequest webRequest = CreatePostRequest(url);
            //WriteFileToRequest(request.FilePath, webRequest);

            //XDocument response = PerformRequest(webRequest);
            //var abbyyOcrTask = XmlParser.GetTaskStatus(response);
            //abbyyOcrTask = WaitForTask(abbyyOcrTask);

            //var kycFolderResponsePath = Path.Combine(Environment.CurrentDirectory, "kyc-files-result");
            //Directory.CreateDirectory(kycFolderResponsePath);
            //var path = Path.Combine(kycFolderResponsePath, Path.GetFileNameWithoutExtension(request.FileName) + "-result.xml");
            //DownloadResult(abbyyOcrTask, path);

            var kycFolderResponsePath = Path.Combine(Environment.CurrentDirectory, "kyc-files-result");
            var path = Path.Combine(kycFolderResponsePath, "Passport01-result.xml");

            var serializer = new XmlSerializer(typeof(DocumentType));

            using (var reader = new StreamReader(path))
            {
                var deserializedResponse = (DocumentType)serializer.Deserialize(reader);
                var dictionary = deserializedResponse.ToDictionary();
                return new MrzProcessResponse(dictionary);
            }
        }

        private OcrTask WaitForTask(OcrTask task)
        {
            Console.WriteLine(string.Format("Task status: {0}", task.Status));
            while (task.IsTaskActive())
            {
                // Note: it's recommended that your application waits
                // at least 2 seconds before making the first getTaskStatus request
                // and also between such requests for the same task.
                // Making requests more often will not improve your application performance.
                // Note: if your application queues several files and waits for them
                // it's recommended that you use listFinishedTasks instead (which is described
                // at https://ocrsdk.com/documentation/apireference/listFinishedTasks/).
                System.Threading.Thread.Sleep(5000);
                task = GetTaskStatus(task.TaskId.Id);
                Console.WriteLine(string.Format("Task status: {0}", task.Status));
            }

            return task;
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

        public void DownloadUrl(string url, string outputFile)
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

            if (url.StartsWith(ServerUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string toEncode = "know-your-customer" + ":" + "+hn8GL6l3pnUl/RGmzkobmPJ";
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

            if (url.StartsWith(ServerUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string toEncode = "know-your-customer" + ":" + "+hn8GL6l3pnUl/RGmzkobmPJ";
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
                //String friendlyMessage = retrieveFriendlyMessage(e);
                //if (friendlyMessage != null)
                //{
                //    throw new ProcessingErrorException(friendlyMessage, e);
                //}
                //throw new ProcessingErrorException("Cannot upload file", e);
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

        public OcrTask GetTaskStatus(string id)
        {
            string url = string.Format("{0}/getTaskStatus?taskId={1}", ServerUrl, Uri.EscapeDataString(id));

            WebRequest request = CreateGetRequest(url);
            XDocument response = PerformRequest(request);
            var serverTask = XmlParser.GetTaskStatus(response);
            return serverTask;
        }
    }
}