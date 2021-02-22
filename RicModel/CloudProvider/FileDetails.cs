using System;

namespace RicModel.CloudProvider
{
    public class FileDetails
    {
        public FileDetails()
        {
        }

        public FileDetails(string bucketName, string key)
        {
            BucketName = bucketName;
            Key = key;
        }

        public string BucketName { get; set; }
        public string Key { get; set; }
        public DateTime LastModified { get; set; }
        public long Size { get; set; }
    }
}
