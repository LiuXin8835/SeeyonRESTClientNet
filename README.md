# 致远OA REST.Net客户端工程说明
本项目主要分为两个工程组成，主工程-**SeeyonClient**和主工程的测试类-**SeeyonClientTest**。     
致远OA的具体REST说明可以参考[致远开放平台REST文档](http://open.seeyon.com/book/ctp/restjie-kou/restjie-kou-java-ke-hu-duan.html)进行查阅
## 主工程
主工程主要提供下列两个类：
### 1.CTPServiceManager
主要用于提供单例对象，如果不想创建直接使用该类即可获取CTPRestClient对象进行使用
### 2.CTPRestClient
主要交互对象，是对HttpClient的一种封装，提供了致远OA整体环境下GET\POST\PUT\DELETE的管理，同时管理了Token，避免了频繁进行认证（用过POSTMAN做测试的都懂）
## 测试工程
测试工程主要对上面的类型检查
### 1.AuthorizeTest
主要对Token进行测试，看看能否正常进行获取，需要先行部署致远OA环境（自己联系致远公司买）
### 2.GPPDTest
主要对CTPRestClient对象进行测试，测试能否正常进行交互动作