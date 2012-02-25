namespace CloudaryStorage.Snda
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("Error")]
    public class SndaError
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Resource { get; set; }

        public Guid RequestId { get; set; }

        public string StringToSign { get; set; }

        public string StringToSignBytes { get; set; }

        public string SignatureProvided { get; set; }

        public string SNDAAccessKeyId { get; set; }
    }
}
