using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Processors
{
    public class AbbyyMrzProcessor : IMrzProcessor
    {
        private readonly HttpClient _httpClient;

        public AbbyyMrzProcessor(HttpClient httpClient)
        {
            _httpClient = Guard.IsNotNull(httpClient, nameof(httpClient));
        }

        public void ProcessMrzFile(MrzProcessRequest request)
        {
            //_httpClient.BaseAddress = new Uri("https://cloud-eu.ocrsdk.com/");
            //_httpClient.PostAsync()

            string url = string.Format("{0}/processMRZ", "https://cloud-eu.ocrsdk.com/");
            WebRequest webRequest = CreatePostRequest(url);
            WriteFileToRequest(request.FilePath, webRequest);

            XDocument response = PerformRequest(webRequest);
            //OcrSdkTask serverTask = ServerXml.GetTaskStatus(response);
            //return serverTask;

           
        }

        private HttpWebRequest CreatePostRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string toEncode = "" + ":" + "";
            string baseEncoded = Convert.ToBase64String(encoding.GetBytes(toEncode));
            request.Headers.Add("Authorization", "Basic " + baseEncoded);

            request.Method = "POST";
            request.ContentType = "application/octet-stream";
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
            catch (System.Net.WebException e)
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
    }
}