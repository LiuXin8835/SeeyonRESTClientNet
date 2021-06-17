using System;
using System.Net.Http;
using System.Text;
using System.Web;
using com.seeyon.client;
using NUnit.Framework;

namespace SeeyonClientTest
{
    /// <summary>
    /// GET/POST/PUT/DELETE方法获取测试
    /// </summary>
    [TestFixture]
    public class GPPDTest
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
        /// 获取测试组织信息
        /// </summary>
        [Test]
        public void TestGetOrgName()
        {
            var client = CTPServiceClientManager.GetInstance(Server_IP,Server_Port,Seeyon_UserName,Seeyon_Password).GetRestClient();
            //这个URL是REST方法获取指定集团内容的链接
            //http://open.seeyon.com/book/ctp/restjie-kou/zu-zhi-mo-xing-guan-li.html
            var result = client.Get($"orgAccount/name/{HttpUtility.UrlEncode("弘扬HIT体验中心", Encoding.UTF8)}/");
            Console.WriteLine("回来的字符串：" + result);
            Assert.IsTrue(!string.IsNullOrEmpty(result) && result.Contains("弘扬HIT体验中心"));
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public com.seeyon.client.CTPRestClient getClient()
        {
            return new com.seeyon.client.CTPRestClient(Server_IP, Server_Port, Seeyon_UserName, Seeyon_Password);
        }


        /// <summary>
        /// 测试创建部门
        /// </summary>
        [Test]
        public void TestCreateDept()
        {
            var client = getClient();
            //这个URL是REST方法获取指定集团内容的链接
            //http://open.seeyon.com/book/ctp/restjie-kou/zu-zhi-mo-xing-guan-li.html
            //故意传空，回来应该是失败对象
            var result = client.Post("orgDepartment/", string.Empty);
            Console.WriteLine("回来的字符串：" + result);
            //HTTP状态应该是415，识别失败
            Assert.IsTrue(string.IsNullOrEmpty(result));
            var json =
                "{\"orgAccountId\":7504955699184944920,\"name\":\"测试三号部门\",\"code\":\"\",\"enabled\":true,\"superior\":7504955699184944920,\"superiorName\":\"弘扬HIS体验中心\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            result = client.Post("orgDepartment/", content);
            Console.WriteLine("回来的字符串：" + result);
            //返回应该是500或者200，添加完成或者同一级别不允许添加相同名称部门
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        /// <summary>
        /// 测试修改部门（PUT方法）
        /// </summary>
        [Test]
        public void TestModifiedDepartment()
        {
            var client = getClient();
            //这个URL是REST方法获取指定集团内容的链接
            //http://open.seeyon.com/book/ctp/restjie-kou/zu-zhi-mo-xing-guan-li.html
            //故意传空，回来应该是失败对象
            var json = "{\"id\":-4804489880897303925,     \"code\": \"\",     \"name\":\"赋能三号部门\" }";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = client.Put("orgDepartment/", content);
            Console.WriteLine("回来的字符串：" + result);
            //HTTP状态应该是500或者200，并且有东西
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
        
        /// <summary>
        /// 测试删除部门（Delete方法）
        /// </summary>
        [Test]
        public void TestDeleteDepartment()
        {
            var client = getClient();
            //这个URL是REST方法获取指定集团内容的链接
            //http://open.seeyon.com/book/ctp/restjie-kou/zu-zhi-mo-xing-guan-li.html
            //故意传空，回来应该是失败对象
            var result = client.Delete("orgDepartment/-1791541751845010743/");
            Console.WriteLine("回来的字符串：" + result);
            //HTTP状态应该是500或者200，并且有东西
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
        
    }
}