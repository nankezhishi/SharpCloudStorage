namespace CloudaryStorage
{
    using System.IO;
    using CloudaryStorage.Model;

    /// <summary>
    /// 
    /// </summary>
    public interface ICloudaryStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ListBucketsResult GetAllBuckets();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        bool CreateBucket(string bucketName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        bool DeleteBucket(string bucketName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        bool PutObject(string bucketName, string objectName, Stream content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Stream GetObject(string bucketName, string objectName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        bool DeleteObject(string bucketName, string objectName);
    }
}
