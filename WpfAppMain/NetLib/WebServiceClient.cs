using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public class WebServiceClient
    {
        private WebServiceClient() { }
        private static WebServiceClient _instance = null;
        public static WebServiceClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WebServiceClient();
                }
                return _instance;
            }
        }

        HttpClient _httpClient;
        public HttpClient httpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                    _httpClient = new HttpClient(handler);
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");   // HTTP KeepAlive设为false，防止HTTP连接保持  
                    _httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");

                    _httpClient.Timeout = new TimeSpan(0, 0, 5);
                    //_httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("GZIP"));
                }
                return _httpClient;
            }
            set { _httpClient = value; }
        }

      

        /// <summary>
        /// Get方式获取数据
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public string GetResult(string httpUrl)
        {
            HttpResponseMessage response = null;

            response = httpClient.GetAsync(httpUrl).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// Get 异步方式获取数据
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public async Task<string> GetAsyncResult(string httpUrl)
        {
            HttpResponseMessage response = null;
            response = await httpClient.GetAsync(httpUrl).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// Post 方式传递数据
        /// </summary>
        /// <param name="postModel"></param>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public string PostResult(string httpUrl, HttpContent postModel)
        {
            HttpResponseMessage response = null;

            if (httpUrl.StartsWith("https"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }
            response = httpClient.PostAsync(httpUrl, postModel).Result;
            if (response != null && response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            return null;

        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        ///  Post 异步方式传递数据
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<string> PostAsyncResult(string httpUrl, HttpContent postModel)
        {
            HttpResponseMessage response = null;

            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            response = await httpClient.PostAsync(httpUrl, postModel).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }
        /// <summary>
        /// Put 更新方式传递数据
        /// </summary>
        /// <param name="postModel"></param>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public string PutResult(string httpUrl, HttpContent postModel)
        {
            HttpResponseMessage response = null;

            response = httpClient.PutAsync(httpUrl, postModel).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }
        /// <summary>
        /// Delete 删除方式传递数据
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public string DeleteResult(string httpUrl)
        {
            HttpResponseMessage response = null;

            response = httpClient.DeleteAsync(httpUrl).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }
       

    }
}
