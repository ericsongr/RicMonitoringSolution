using System;
using System.Net;
using Amazon.S3.Model;
using RicModel.CloudProvider;

namespace RicAmazonS3.DataAdapter
{
    public static class UploadResponseAdapter
    {
        public static Func<PutObjectResponse, UploadResponse> PutObjectResponseToUploadResponse = r =>
            r == null ? null : new UploadResponse
            {
                Success = r.HttpStatusCode == HttpStatusCode.OK
            };
    }
}
