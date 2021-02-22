using System;
using System.Collections.Generic;
using System.Text;

namespace RicAmazonS3.Models
{
    public class ServiceConfiguration
    {
        public AWSS3Configuration AWSS3 { get; set; }
    }

    public class AWSS3Configuration
    {
        public string BucketName { get; set; }
    }
}
