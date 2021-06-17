namespace SeeyonClient.Model
{
    /// <summary>
    /// Token对象
    /// </summary>
    public class TokenObj
    {
        /// <summary>
        /// 绑定的用户
        /// </summary>
        public string bindingUser { get; set; }

        /// <summary>
        /// 回来的Token
        /// </summary>
        public string id { get; set; }
    }
}