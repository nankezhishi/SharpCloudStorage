namespace CloudaryStorage.Ali
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using CloudaryStorage.Model;

    /// <summary>
    /// 
    /// </summary>
    [Export("Ali", typeof(ICloudaryStorage))]
    public class AliCloudStorage : ICloudaryStorage
    {
        public ListBucketsResult GetAllBuckets()
        {
            throw new NotImplementedException();
        }

        public bool CreateBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        public bool PutObject(string bucketName, string objectName, Stream content)
        {
            throw new NotImplementedException();
        }

        public Stream GetObject(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteObject(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }
    }
}
