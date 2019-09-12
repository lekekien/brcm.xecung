using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Utility
{
    public class ApiUtils
    {
        public static string MakePostRequest(string url, string data, string contentType = "application/json", bool keepAlive = true)
        {
            try
            {
                //Logger.GetInstance().Write(url);

                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = string.IsNullOrEmpty(contentType) ? "application/x-www-form-urlencoded" : contentType;
                request.ContentLength = byteArray.Length;
                request.KeepAlive = keepAlive;
                request.Timeout = Timeout.Infinite;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                //Get response
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromApi = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromApi;
            }
            catch (Exception ex)
            {
                //Logger.GetInstance().Write(url);
                //Logger.GetInstance().Write(string.Format("Make Post Request: {0}", ex.Message));
                //Logger.GetInstance().Write(string.Format("Make Post Request: {0}", ex.StackTrace));
                return string.Empty;
            }
        }
    }
}
