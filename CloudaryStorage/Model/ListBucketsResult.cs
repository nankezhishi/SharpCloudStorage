namespace CloudaryStorage.Model
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("ListAllMyBucketsResult")]
    public class ListBucketsResult
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Buckets")]
        [XmlArrayItem("Bucket")]
        public List<Bucket> Buckets { get; set; }
    }
}
