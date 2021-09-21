using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using DeUrgenta.Certifications.Api.Storage.Config;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Certifications.Api.Storage
{
    public class S3Storage : IBlobStorage
    {
        private readonly S3ConfigOptions _config;
        private readonly IAmazonS3 _s3Client;

        public S3Storage(IOptionsMonitor<S3ConfigOptions> config, IAmazonS3 client)
        {
            _s3Client = client;
            _config = config.CurrentValue;
        }

        public async Task<string> SaveAttachmentAsync(Guid certificationId, string userSub, Stream attachment)
        {
             var fileKey = $"{userSub}/{certificationId}";
            var request = new PutObjectRequest
            {
                BucketName = _config.BucketName,
                Key = fileKey,
                InputStream = attachment
            };
            await _s3Client.PutObjectAsync(request);
            
            return GetPreSignedUrl(fileKey);
        }

        public string GetAttachment(Guid certificationId, string userSub)
        {
            var fileKey = $"{userSub}/{certificationId}";
            return GetPreSignedUrl(fileKey);
        }

        private string GetPreSignedUrl(string fileKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _config.BucketName, 
                Key = fileKey,
                Expires = DateTime.Now.AddMinutes(_config.PresignedUrlExpirationInMinutes)
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}