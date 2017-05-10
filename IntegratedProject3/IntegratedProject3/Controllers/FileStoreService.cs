using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Controllers
{
    public class FileStoreService
    {
        private const string bucketName = "cwktest";
        private const string AWS_ACCESS_KEY = "";

        /// <summary>
        /// Stores a file on the data storage center
        /// </summary>
        /// <param name="file">The File to be Stored</param>
        /// <returns>returns the key to be stored</returns>
        public string UploadFile(HttpPostedFileBase file)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = bucketName,
                    Key = Guid.NewGuid().ToString(),
                    InputStream = file.InputStream,
                    ContentType = "Document"
                };

                var response = client.PutObject(putRequest);

                return putRequest.Key;
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

        /// <summary>
        /// Gets the file from the bucket based on the key entered
        /// </summary>
        /// <param name="key">The File Key</param>
        /// <returns>The Stream of the file stored. </returns>
        public GetObjectResponse GetFile(string key)
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1))
            {
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key
                };

                var response = client.GetObject(request);

            
                return response;
            }
        }

    }

}
