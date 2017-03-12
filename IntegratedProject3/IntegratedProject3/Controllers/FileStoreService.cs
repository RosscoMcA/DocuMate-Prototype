using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Controllers
{
    public class FileStoreService
    {

        private const string bucketName = "";
        private const string AWS_ACCESS_KEY = "";

        public void UploadFile(HttpPostedFile file)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    CannedACL = S3CannedACL.PublicReadWrite,
                    BucketName = bucketName,
                    Key = AWS_ACCESS_KEY,
                    InputStream = file.InputStream,
                    ContentType = "Document"
                };

                var response = client.PutObject(putRequest);

               
            }
            catch (AmazonS3Exception e)
            {
                if (e.ErrorCode != null &&
                    (e.ErrorCode.Equals("InvalidAccessKeyId")
                    || e.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials");
                }
                else
                {
                    throw new Exception("Error occured: " + e.Message);
                }
            }
        }

    }
}