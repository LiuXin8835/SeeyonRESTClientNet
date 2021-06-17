using System;
using NUnit.Framework;

namespace SeeyonClientTest
{
    /// <summary>
    /// 认证测试
    /// </summary>
    [TestFixture]
    public class AuthorizeTest
    {
        /// <summary>
        /// 服务器IP（换成你自己的IP）
        /// </summary>
        private static string Server_IP = "127.0.0.1";
        /// <summary>
        /// 服务器端口（换成你自己服务器的端口）
        /// </summary>
        private static string Server_Port = "6700";

        /// <summary>
        /// 换成你自己就致远OA的用户名
        /// </summary>
        private static string Seeyon_UserName = "HYOA";

        /// <summary>
        /// 换成你自己从致远OA里面拿到的密码
        /// </summary>
        private static string Seeyon_Password = "c56e965f-6d85-41d0-aa22-3c3ea4215c67";
        

        /// <summary>
        /// 测试获取Token
        /// </summary>
        [Test]
        public void TestGetToken()
        {
            var client = getClient();
            var token = client.Authenticate();
            Console.WriteLine("获取到的Token:"+token);
            Assert.IsTrue(!String.IsNullOrEmpty(token));
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public com.seeyon.client.CTPRestClient getClient()
        {
            return new com.seeyon.client.CTPRestClient(Server_IP,Server_Port,Seeyon_UserName,Seeyon_Password);
        }
    }
}