namespace CloudaryStorage.Snda
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Xml.Serialization;
    using CloudaryStorage.Model;

    /// <summary>
    /// 
    /// </summary>
    [Export("SNDA", typeof(ICloudaryStorage))]
    public class SndaCloudStorage : ICloudaryStorage
    {
        /// <summary>
        /// 
        /// </summary>
        public SndaCloudStorage()
        {
            BaseUrl = "http://storage.grandcloud.cn";
            AccessKey = "ivhcemouk9rc24w5w";
            SecretAccessKey = "MzNhODExZDVmMDBlMDFmNThhNWRmOWI0OTc4ZjBmZTE=";
        }

        /// <summary>
        /// 服务器的基地址
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 从云存储上申请的AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 从云存储上申请的SecretAccessKey
        /// </summary>
        public string SecretAccessKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ListBucketsResult GetAllBuckets()
        {
            var response = MakeBucketRequest("/", WebRequestMethods.Http.Get);

            return ExtractData<ListBucketsResult>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public bool CreateBucket(string bucketName)
        {
            var response = MakeBucketRequest("/" + bucketName, WebRequestMethods.Http.Put);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new SndaCloudException(ExtractData<SndaError>(response));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public bool DeleteBucket(string bucketName)
        {
            var response = MakeBucketRequest("/" + bucketName, "DELETE");
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new SndaCloudException(ExtractData<SndaError>(response));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool PutObject(string bucketName, string objectName, Stream content)
        {
            var response = MakeBucketRequest("/" + bucketName + "/" + objectName, WebRequestMethods.Http.Put, content);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new SndaCloudException(ExtractData<SndaError>(response));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public Stream GetObject(string bucketName, string objectName)
        {
            var response = MakeBucketRequest("/" + bucketName + "/" + objectName, WebRequestMethods.Http.Get);

            return response.GetResponseStream();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public bool DeleteObject(string bucketName, string objectName)
        {
            var response = MakeBucketRequest("/" + bucketName + "/" + objectName, "DELETE");
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new SndaCloudException(ExtractData<SndaError>(response));
            }
        }

        private HttpWebResponse MakeBucketRequest(string path, string method, Stream content = null)
        {
            var datestring = DateTime.Now.ToUniversalTime().ToString("r");

            var canonicalizedSNDAHeaders = String.Empty;
            var stringToSign =
                method + "\n"
                + "\n"                       // Content-Md5 Filed
                + "\n"                       // Content-Type Filed
                + datestring + "\n"          // Date Filed
                + canonicalizedSNDAHeaders
                + path;

            var sign = SignatureHelper.Sign(stringToSign, SecretAccessKey);
            var authorization = SignatureHelper.Authorization(AccessKey, sign);

            var request = WebRequest.Create(BaseUrl + path) as HttpWebRequest;
            request.Headers.Add("Authorization", authorization);
            request.Date = DateTime.Now;
            request.Method = method;
            if (content != null)
            {
                if (method != WebRequestMethods.Http.Put)
                    throw new ArgumentException("发送流数据的请求，应当使用PUT方法。");

                if (!content.CanSeek)
                    throw new ArgumentException("无法从参数content中获取数据的大小。");

                request.ContentLength = content.Length;
                content.CopyTo(request.GetRequestStream());
            }

            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                return e.Response as HttpWebResponse;
            }
        }

        private T ExtractData<T>(WebResponse reponse) where T : class
        {
            using (var stream = reponse.GetResponseStream())
            {
#if DEBUG
                var cacheStream = new MemoryStream();
                stream.CopyTo(cacheStream);
                cacheStream.Seek(0, SeekOrigin.Begin);
#else
                cacheStream = stream;
#endif

                var serializer = new XmlSerializer(typeof(T));
                try
                {
                    var result = serializer.Deserialize(cacheStream) as T;

                    return result;
                }
                catch (InvalidOperationException)
                {
#if DEBUG
                    cacheStream.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(cacheStream);
                    Trace.TraceError(sr.ReadToEnd());

                    cacheStream.Seek(0, SeekOrigin.Begin);
                    serializer = new XmlSerializer(typeof(SndaError));
                    throw new SndaCloudException(serializer.Deserialize(cacheStream) as SndaError);
#else
                    throw;
#endif
                }
            }
        }
    }
}
