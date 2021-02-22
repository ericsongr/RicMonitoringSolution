using System;
using Amazon.S3.Model;
using RicModel.CloudProvider;

namespace RicAmazonS3.DataAdapter
{
    public static class FileDetailsAdapter
    {
        public static Func<S3Object, FileDetails> UploadRequestToPutObjectRequest = o =>
            o == null ? null : new FileDetails
            {
                BucketName = o.BucketName,
                Key = o.Key,
                LastModified = o.LastModified,
                Size = o.Size
            };

        public static Func<FileDetails, GetPreSignedUrlRequest> FileDetailsToGetPresignedUrlRequest = o =>
            o == null ? null : new GetPreSignedUrlRequest
            {
                BucketName = o.BucketName,
                Key = o.Key,
                Expires = DateTime.Now.AddMinutes(30)
            };

        public static Func<FileDetails, DeleteObjectRequest> FileDetailsToDelectObjectRequest = o =>
            o == null ? null : new DeleteObjectRequest
            {
                BucketName = o.BucketName,
                Key = o.Key
            };
    }
}
