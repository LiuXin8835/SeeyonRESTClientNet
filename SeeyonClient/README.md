# 致远OA REST客户端(.Net版)
致远OA REST客户端（.Net版）是对致远OA REST接口的一种封装，设计上借鉴了致远OA REST Java客户端的设计，同时又加入 .Net 的元素，是对其的一种封装升华。

## 依赖项
1. .Net Framework 4.6.1
2. Newtonsoft.Json

## 类
### CTPServiceClientManager
与官网一样提供一个静态的实例类型，可以获取管理对象，来获取Rest客户端
#### 方法：
(静态方法)GetInstance   
GetRestClient     

使用示例
``` C#
//使用IP地址和端口号进行构造，同时传入用户名和密码
var client = CTPServiceClientManager.GetInstance(Server_IP,Server_Port,Seeyon_UserName,Seeyon_Password).GetRestClient();
//使用IP地址和端口号进行构造，不传入用户密码
//使用时需要调用client.Authenticate(userName, password);
var client = CTPServiceClientManager.GetInstance(Server_IP,Server_Port).GetRestClient();
//使用服务器地址进行构造，服务器地址不能以"/"结尾，同时传入用户名和密码，使用时不需要验证
var client = CTPServiceClientManager.GetInstance(Server_Address,Seeyon_UserName,Seeyon_Password).GetRestClient();
//使用服务器地址进行构造，服务器地址不能以"/"结尾
//使用时需要调用client.Authenticate(userName, password);
var client = CTPServiceClientManager.GetInstance(Server_Address).GetRestClient();
```

### CTPRestClient
对于HttpClient的封装，主要根据REST API要求提供Get\Post\Put\Delete方法，以及提供getToken和认证操作。     
.Net封装版不提供BindUser方法（因为不懂只传入用户名称是什么操作）

#### 方法
Authenticate    
Get    
Post     
Put     
Delete   
GetHttpClient    
GetToken

示例：
``` C#
 /// <summary>
        /// 获取测试组织信息
        /// </summary>
        [Test]
        public void TestGetOrgName()
        {
            var client = CTPServiceClientManager.GetInstance(Server_IP,Server_Port,Seeyon_UserName,Seeyon_Password).GetRestClient();
            //这个URL是REST方法获取指定集团内容的链接
            //http://open.seeyon.com/book/ctp/restjie-kou/zu-zhi-mo-xing-guan-li.html
            var result = client.Get($"orgAccount/name/{HttpUtility.UrlEncode("集团名称", Encoding.UTF8)}/");
            Console.WriteLine("回来的字符串：" + result);
            Assert.IsTrue(!string.IsNullOrEmpty(result) && result.Contains("集团名称"));
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
                "{\"orgAccountId\":7504955699184944920,\"name\":\"测试三号部门\",\"code\":\"\",\"enabled\":true,\"superior\":7504955699184944920,\"superiorName\":\"集团名称\"}";
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
```

