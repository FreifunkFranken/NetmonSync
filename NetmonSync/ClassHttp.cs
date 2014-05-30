using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;

namespace NetmonSync
{
    /// <summary>
    /// Klasse für einfache HTTP Befehle
    /// </summary>
    public static class ClassHttp
    {
        public static string HttpGet(string url, Dictionary<string, string> parameters)
        {
            url += "?" + combineParameters(parameters);
            return HttpGet(url);
        }
        public static string HttpGet(string url)
        {
            //I use a method to ignore bad certs caused by misc errors
            IgnoreBadCertificates();

            WebRequest MyRequest;
            MyRequest = WebRequest.Create(url);
            MyRequest.Method = "GET";

            Stream ResponseStream;
            ResponseStream = MyRequest.GetResponse().GetResponseStream();
            StreamReader ResponseReader = new StreamReader(ResponseStream);

            string TempLine = "";
            int i = 0;
            string result = "";
            while (TempLine != null)
            {
                i++;
                TempLine = ResponseReader.ReadLine();
                if (TempLine != null)
                    result += TempLine;
            }

            return result;
        }
        public static string HttpPost(string url, string data)
        {
            try {
                //I use a method to ignore bad certs caused by misc errors
                IgnoreBadCertificates();

                WebRequest request;
                request = WebRequest.Create(url);
                request.Method = "POST";

                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                Stream datastream = ex.Response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                Console.WriteLine(sr.ReadToEnd());
                return "";
            }
        }
        public static string HttpPut(string url, string data)
        {
            try
            {
                //I use a method to ignore bad certs caused by misc errors
                IgnoreBadCertificates();

                WebRequest request;
                request = WebRequest.Create(url);
                request.Method = "PUT";

                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    switch (response.StatusCode)
                //    {
                //        case HttpStatusCode.OK: /* 200 */
                //            {
                //                dataStream = response.GetResponseStream();
                //                StreamReader reader = new StreamReader(dataStream);
                //                string responseFromServer = reader.ReadToEnd();
                //                //Console.WriteLine(responseFromServer);
                //                reader.Close();
                //                dataStream.Close();
                //                response.Close();

                //                return responseFromServer;
                //            }
                //        default:
                //            {
                //                Console.WriteLine(string.Format("Status Code: {0}", (int)response.StatusCode));
                //                return "";
                //            }
                //    }
                //}

                WebResponse response = request.GetResponse();
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                Stream datastream = ex.Response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                Console.WriteLine(sr.ReadToEnd());
                return "";
            }
        }
        public static bool HttpDelete(string url)
        {
            try
            {
                //I use a method to ignore bad certs caused by misc errors
                IgnoreBadCertificates();

                WebRequest MyRequest;
                MyRequest = WebRequest.Create(url);
                MyRequest.Method = "DELETE";

                WebResponse Response = MyRequest.GetResponse();

                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                Stream datastream = ex.Response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                Console.WriteLine(sr.ReadToEnd());
                return false;
            }
        }

        private static string combineParameters(Dictionary<string, string> parameters)
        {
            string result = "";
            foreach(var parameter in parameters)
            {
                if (result != "") result += "&";
                result += parameter.Key + "=" + parameter.Value;
            }
            return result;
        }

        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }  

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        } 
    }
}
