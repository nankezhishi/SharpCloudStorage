namespace CloudaryStorage.Snda
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class SignatureHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string Sign(string data, string secretKey)
        {
            if (String.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("请确认secretKey是否设置正确");
            if (String.IsNullOrEmpty(data))
                throw new ArgumentNullException("数据不能为空");

            var sha = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));

            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string Authorization(string accessKey, string sign)
        {
            if (String.IsNullOrEmpty(accessKey))
                throw new ArgumentNullException("请确认accessKey是否设置正确");

            if (String.IsNullOrEmpty(sign))
                throw new ArgumentNullException("签名不能为空");

            return "SNDA" + " " + accessKey + ":" + sign;
        }
    }
}
