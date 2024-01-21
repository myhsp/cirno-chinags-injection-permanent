using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class HttpTransmissionManager
    {

        public HttpTransmissionManager()
        {
        }

		public string Get(string api, ref bool hasError)
        {
			hasError = true;
			string result = string.Empty;
			string url = string.Concat(Program.Config.RemoteHttpConfig.HttpServerAddr, "/", api);
			WebRequest webRequest = WebRequest.Create(url);
			webRequest.ContentType = "application/json";
			webRequest.Method = "GET";
			webRequest.Timeout = Program.Config.RemoteHttpConfig.Timeout;
			try
			{
				using (Stream responseStream = webRequest.GetResponse().GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						result = streamReader.ReadToEnd();
						hasError = false;
					}
				}
				return result;
			}
            catch
            {
				hasError = true;
				return result;
            }
		}

        public string Post(string api, string body, ref bool hasError)
        {
			hasError = true;
			string result = string.Empty;
			string url = string.Concat(Program.Config.RemoteHttpConfig.HttpServerAddr, "/", api);
            try
            {
				WebRequest webRequest = WebRequest.Create(url);
				webRequest.ContentType = "application/json";
				webRequest.Method = "POST";
				webRequest.Timeout = Program.Config.RemoteHttpConfig.Timeout;
				if (body != null)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(body);
					using (Stream requestStream = webRequest.GetRequestStream())
					{
						requestStream.Write(bytes, 0, bytes.Length);
					}
				}
				using (WebResponse response = webRequest.GetResponse())
				{
					using (Stream responseStream = response.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(responseStream))
						{
							result = streamReader.ReadToEnd();
							hasError = false;
							streamReader.Close();
						}
						responseStream.Close();
					}
					response.Close();
					return result;
				}
			}
            catch
            {
				hasError = true;
				return result;
            }
		}
    }
}
