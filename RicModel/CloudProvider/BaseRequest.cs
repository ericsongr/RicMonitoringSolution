namespace RicModel.CloudProvider
{
    public class BaseRequest
    {
        /// <summary>
        ///     Gets or sets the name of the bucket or container used to aggrigate storage at the top level. e.g. Amazong S3 Bucket
        ///     Name
        /// </summary>
        public string BucketName { get; set; } = "objectdatatest";

        /// <summary>
        ///     Gets or sets the key used to store the file, like directory path
        /// </summary>
        public string Key { get; set; }
    }
}
