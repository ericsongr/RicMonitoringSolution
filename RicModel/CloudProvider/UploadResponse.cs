using System;

namespace RicModel.CloudProvider
{
    public class UploadResponse
    {
        public bool Success { get; set; }
        public Uri ResourceUri { get; set; }
    }
}
