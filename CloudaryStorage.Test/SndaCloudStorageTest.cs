namespace CloudaryStorage.Test
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using CloudaryStorage.Snda;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// 
    /// </summary>
    [TestClass()]
    public class SndaCloudStorageTest
    {
        /// <summary>
        /// The value will be composited. So disable not assign to warning.
        /// </summary>
#pragma warning disable 0649
        [Import("SNDA", typeof(ICloudaryStorage))]
        private ICloudaryStorage storage;
#pragma warning restore 0649

        [TestInitialize]
        public void Initialize()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ICloudaryStorage).Assembly));
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Trace.WriteLine(compositionException.ToString());
            }
        }

        /// <summary>
        /// 测试获取Bucket列表
        /// </summary>
        [TestMethod]
        public void GetAllBucketsTest()
        {
            var result = storage.GetAllBuckets();
            Assert.IsTrue(result.Buckets.Count > 0);
        }

        /// <summary>
        /// 测试创建、删除Bucket
        /// </summary>
        [TestMethod]
        public void CreateAndDeleteBucketTest()
        {
            var bucketName = "bambookbookcachetest";
            var result = storage.CreateBucket(bucketName);
            Assert.AreEqual(true, result);
            result = storage.DeleteBucket(bucketName);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// 删除一个不存在的Bucket用于测试SndaError
        /// </summary>
        [TestMethod]
        public void DeleteNoneExistBucketTest()
        {
            var bucketName = "bambookbookcachetestthatnotexists";
            try
            {
                var result = storage.DeleteBucket(bucketName);
            }
            catch (SndaCloudException e)
            {
                Assert.IsNotNull(e.ErrorInfo);
                Assert.AreEqual("NoSuchBucket", e.ErrorInfo.Code);
            }
        }

        /// <summary>
        /// 测试在Bucket中保存数据。
        /// </summary>
        [TestMethod]
        public void CreateObjectTest()
        {
            var bucketName = "bambookbookcache";
            var objectName = "AppId_BookId_PageNumer_Format";
            var data = "Data";
            // 建立对象
            var result = storage.PutObject(bucketName, objectName, new MemoryStream(Encoding.Default.GetBytes(data)));
            Assert.AreEqual(true, result);
            // 获取对象
            var refetchedData = new StreamReader(storage.GetObject(bucketName, objectName), Encoding.Default).ReadToEnd();
            Assert.AreEqual(data, refetchedData);
            // 删除对象
            result = storage.DeleteObject(bucketName, objectName);
            Assert.AreEqual(true, result);
        }
    }
}
