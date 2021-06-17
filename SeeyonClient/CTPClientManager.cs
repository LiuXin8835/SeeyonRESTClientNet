namespace com.seeyon.client
{
    public class CTPServiceClientManager
    {
        private static CTPServiceClientManager _clientManager = null;
        private CTPRestClient _client;

        private CTPServiceClientManager(string address)
        {
            this._client = new CTPRestClient(address);
        }

        private CTPServiceClientManager(string address, string userName, string password)
        {
            this._client = new CTPRestClient(address, userName, password);
        }

        private CTPServiceClientManager(string ip, string port)
        {
            this._client = new CTPRestClient(ip, port);
        }

        private CTPServiceClientManager(string ip, string port, string username, string password)
        {
            this._client = new CTPRestClient(ip, port, username, password);
        }

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CTPServiceClientManager GetInstance(string address)
        {
            if (_clientManager == null)
            {
                _clientManager = new CTPServiceClientManager(address);
            }

            return _clientManager;
        }

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CTPServiceClientManager GetInstance(string address, string userName, string password)
        {
            if (_clientManager == null)
            {
                _clientManager = new CTPServiceClientManager(address, userName, password);
            }

            return _clientManager;
        }

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static CTPServiceClientManager GetInstance(string ip, string port)
        {
            if (_clientManager == null)
            {
                _clientManager = new CTPServiceClientManager(ip, port);
            }

            return _clientManager;
        }

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static CTPServiceClientManager GetInstance(string ip, string port, string userName, string password)
        {
            if (_clientManager == null)
            {
                _clientManager = new CTPServiceClientManager(ip, port, userName, password);
            }

            return _clientManager;
        }

        /// <summary>
        /// 获取REST客户端
        /// </summary>
        /// <returns></returns>
        public CTPRestClient GetRestClient()
        {
            return _client;
        }
    }
}