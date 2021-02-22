using System;
using Amazon.S3;
using Amazon.S3.Model;
using RicModel.CloudProvider;

namespace RicAmazonS3.DataAdapter
{
    public static class UploadRequestAdapter
    {
        public static Func<UploadRequest, PutObjectRequest> UploadRequestToPutObjectRequest = r =>
            r == null ? null : new PutObjectRequest
            {
                BucketName = r.BucketName,
                Key = r.Key,
                ContentType = r.ContentType,
                CannedACL = r.IsPublicViewable ? S3CannedACL.PublicRead : S3CannedACL.Private,
                FilePath = r.SourceFilePath,
                InputStream = r.InputStream
            };
    }
}
