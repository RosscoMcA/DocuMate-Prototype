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
        ///Create new AWS Client for the DUBLIN Server storage base
        ///Be aware that this is the only version that supports the Dublin server
        private AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);
        private const string bucketName = "cwktest";
        

        /// <summary>
        /// Uploads a file given by the user. 
        /// </summary>
        /// <param name="file">The File inputted into the webpage</param>
        /// <returns>the key to store in the database</returns>
        public string UploadFile(HttpPostedFile file)
        {
            
            
            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    CannedACL = S3CannedACL.PublicReadWrite,
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
        /// Searches for the file required and returns either a 
        /// found file or null.
        /// </summary>
        /// <param name="filekey"></param>
        /// <returns>Returns a stream of the file to be used,
        /// or null if no file was found</returns>
        public Stream getFile(string filekey)
        {

            var candidateObj = new GetObjectRequest()
            {
                Key = filekey,
                BucketName = bucketName
            };
            var file = client.GetObject(candidateObj);
            if (file != null) { 
            Stream fileStream = new MemoryStream();

            file.ResponseStream.CopyTo(fileStream);

            return fileStream;
        }
        else return null;
        }

    }
}