using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using SeeyonClient.Model;

namespace com.seeyon.client
{
    /// <summary>
    /// 致远OA的客户端程序
    /// </summary>
    public class CTPRestClient
    {
        //致远OA服务器的IP地址
        private string Server_IP = "127.0.0.1";

        //致远OA的服务端口
        private string Server_Port = "80";

        //用户名
        private string UserName;

        //密码
        private string Password;

        /// <summary>
        /// 记录更新的时间
        /// </summary>
        private DateTime updateToken;

        /// <summary>
        /// 基础链接
        /// </summary>
        private string baseUrl = null;

        //客户端
        private HttpClient _client;

        /// <summary>
        /// 内部维护的Token
        /// </summary>
        private string Token = null;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CTPRestClient()
        {
            _client = new HttpClient();
            baseUrl = $"http://{Server_IP}:{Server_Port}/seeyon/rest/";
        }

        /// <summary>
        /// 带参数构造（包含用户名）
        /// </summary>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="serverPort">服务器端口</param>
        public CTPRestClient(string serverIp, string serverPort, string userName, string password)
        {
            Server_IP = serverIp;
            Server_Port = serverPort;
            UserName = userName;
            Password = password;
            _client = new HttpClient();
            baseUrl = $"http://{Server_IP}:{Server_Port}/seeyon/rest/";
        }

        /// <summary>
        /// 带参数构造（不包含用户名）
        /// </summary>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="serverPort">服务器端口</param>
        public CTPRestClient(string serverIp, string serverPort)
        {
            Server_IP = serverIp;
            Server_Port = serverPort;
            _client = new HttpClient();
            baseUrl = $"http://{Server_IP}:{Server_Port}/seeyon/rest/";
        }

        /// <summary>
        /// 官网URL构造
        /// </summary>
        /// <param name="url"></param>
        public CTPRestClient(string url)
        {
            _client = new HttpClient();
            baseUrl = $"{url}/seeyon/rest/";
        }

        /// <summary>
        /// 官网URL构造
        /// </summary>
        /// <param name="url"></param>
        public CTPRestClient(string url, string userName, string password)
        {
            _client = new HttpClient();
            UserName = userName;
            Password = password;
            baseUrl = $"{url}/seeyon/rest/";
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            if (string.IsNullOrEmpty(Token)||checkNeedUpdateToken())
            {
                return Authenticate();
            }
            else
            {
                return Token;
            }
        }

        /// <summary>
        /// 使用默认的用户名和密码进行验证
        /// </summary>
        /// <returns></returns>
        public string Authenticate()
        {
            var result = _client.GetAsync($"{baseUrl}token/{UserName}/{Password}");
            result.Wait();
            if (result.Result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                var strResult = result.Result.Content.ReadAsStringAsync();
                var json = strResult.Result;
                var tokenObj = JsonConvert.DeserializeObject<TokenObj>(json);
                if (tokenObj != null)
                {
                    this.updateToken = DateTime.Now;
                    this.Token = tokenObj.id;
                    return tokenObj.id;
                }
            }

            return null;
        }


        /// <summary>
        /// 使用指定用户名和密码进行验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(string userName, string password)
        {
            var result = _client.GetAsync($"{baseUrl}/token/{userName}/{password}");
            this.UserName = userName;
            this.Password = password;
            result.Wait();
            if (result.Result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                var strResult = result.Result.Content.ReadAsStringAsync();
                var json = strResult.Result;
                var tokenObj = JsonConvert.DeserializeObject<TokenObj>(json);
                if (tokenObj != null && !string.IsNullOrEmpty(tokenObj.id))
                {
                    this.updateToken = DateTime.Now;
                    this.Token = tokenObj.id;
                    return tokenObj.id;
                }
            }

            return null;
        }


        /// <summary>
        /// Get方式获取结果
        /// </summary>
        /// <param name="url">输入的人URL（不用输前面的致远OA之类的）</param>
        /// <returns>结果的JSON形式字符串，如果错误则返回空</returns>
        public string Get(string url)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.GetAsync(Url);
            result.Wait();
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Get方式获取结果
        /// </summary>
        /// <param name="url">输入的人URL（不用输前面的致远OA之类的）</param>
        /// <typeparam name="T">JSON转换后的结果（可能会抛异常）</typeparam>
        /// <returns></returns>
        public T Get<T>(String url)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.GetAsync(Url);
            result.Wait();
            return JsonConvert.DeserializeObject<T>(result.Result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">HTTPContent的Body</param>
        /// <returns></returns>
        public string Post(string url, HttpContent body)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.PostAsync(Url, body);
            result.Wait();
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">HTTPContent的Body</param>
        /// <returns></returns>
        public T Post<T>(string url, HttpContent body)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.PostAsync(Url, body);
            result.Wait();
            return JsonConvert.DeserializeObject<T>(result.Result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">字符串对象</param>
        /// <returns></returns>
        public string Post(string url, string body, string contentType = "application/json")
        {
            HttpContent conent = new StringContent(body, Encoding.UTF8, contentType);
            return Post(url, conent);
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">字符串对象</param>
        /// <returns></returns>
        public T Post<T>(string url, string body, string contentType = "application/json")
        {
            HttpContent conent = new StringContent(body, Encoding.UTF8, contentType);
            return Post<T>(url, conent);
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="O">实体对象O</param>
        /// <returns></returns>
        public string Post(string url, Object O)
        {
            return Post(url, JsonConvert.SerializeObject(O));
        }

        /// <summary>
        /// POST方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="O">实体对象O</param>
        /// <returns></returns>
        public T Post<T>(string url, Object O)
        {
            return Post<T>(url, JsonConvert.SerializeObject(O));
        }

        /// <summary>
        /// PUT方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">HTTP对象(默认编码为UTF-8)</param>
        /// <returns></returns>
        public string Put(string url, HttpContent body)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.PutAsync(Url, body);
            result.Wait();
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// PUT方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">HTTP对象(默认编码为UTF-8)</param>
        /// <returns></returns>
        public T Put<T>(string url, HttpContent body)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.PutAsync(Url, body);
            result.Wait();
            return JsonConvert.DeserializeObject<T>(result.Result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// PUT方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">字符串对象(默认编码为UTF-8)</param>
        /// <returns></returns>
        public string Put(string url, string body, string contentType = "application/json")
        {
            return Put(url, new StringContent(body, Encoding.UTF8, contentType));
        }

        /// <summary>
        /// PUT方法发送内容
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="body">字符串对象(默认编码为UTF-8)</param>
        /// <returns></returns>
        public T Put<T>(string url, string body, string contentType = "application/json")
        {
            return Put<T>(url, new StringContent(body, Encoding.UTF8, contentType));
        }

        /// <summary>
        /// PUT方式发送对象
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="o">实体对象</param>
        /// <returns></returns>
        public string Put(string url, Object o)
        {
            return Put(url, JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// PUT方式发送对象
        /// </summary>
        /// <param name="url">链接的URL</param>
        /// <param name="o">实体对象</param>
        /// <typeparam name="T">要转换成的类型T</typeparam>
        /// <returns></returns>
        public T Put<T>(string url, Object o)
        {
            return Put<T>(url, JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// DELETE方法发送内容
        /// </summary>
        /// <param name="url">链接URL</param>TestCreatePostDeptch创建
        /// <returns></returns>
        public string Delete(string url)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.DeleteAsync(Url);
            result.Wait();
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// DELETE方法发送内容
        /// </summary>
        /// <param name="url">链接URL</param>TestCreatePostDeptch创建
        /// <returns></returns>
        public T Delete<T>(string url)
        {
            if (string.IsNullOrEmpty(Token) || checkNeedUpdateToken())
            {
                Authenticate();
            }

            string Url = $"{baseUrl}{url}{(url.EndsWith("/") ? ($"?token={Token}") : ($"&token={Token}"))}";
            var result =
                _client.DeleteAsync(Url);
            result.Wait();
            return JsonConvert.DeserializeObject<T>(result.Result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// 检查是否需要更新Token，Token的有效期是15分钟
        /// </summary>
        /// <returns></returns>
        private bool checkNeedUpdateToken()
        {
            return DateTime.Now.Subtract(this.updateToken).TotalMinutes > 14;
        }

        /// <summary>
        ///获取HTTP客户端
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHTTPClient()
        {
            return _client;
        }

        /// <summary>
        /// 获取基础的URL
        /// </summary>
        /// <returns></returns>
        public string GetBaseURL()
        {
            return baseUrl;
        }
    }
}