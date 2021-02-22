using System;
using System.IO;

namespace RicModel.CloudProvider
{
    public class UploadRequest : BaseRequest
    {
        public Uri CloudHostUri { get; set; }
        public string SourceFilePath { get; set; }
        public Stream InputStream { get; set; }

        /// <summary>
        ///     Gets or sets the type of the content / MIME Type. e.g. image/jpg
        /// </summary>
        public string ContentType { get; set; }

        public bool IsPublicViewable { get; set; } = true;

        public bool Validate()
        {
            return (!string.IsNullOrWhiteSpace(SourceFilePath) || InputStream != null) &&
                   !string.IsNullOrWhiteSpace(ContentType) &&
                   !string.IsNullOrWhiteSpace(BucketName) &&
                   !string.IsNullOrWhiteSpace(Key);
        }
    }
}
