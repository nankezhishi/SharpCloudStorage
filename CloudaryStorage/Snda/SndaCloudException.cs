namespace CloudaryStorage.Snda
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class SndaCloudException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public SndaCloudException(SndaError error)
        {
            ErrorInfo = error;
        }

        /// <summary>
        /// 
        /// </summary>
        public SndaError ErrorInfo { get; set; }
    }
}
