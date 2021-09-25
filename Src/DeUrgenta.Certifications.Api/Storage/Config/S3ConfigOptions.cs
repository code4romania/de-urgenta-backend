namespace DeUrgenta.Certifications.Api.Storage.Config
{
    public class S3ConfigOptions
    {
        public string BucketName { get; set; }
        public int PresignedUrlExpirationInMinutes { get; set; }
    }
}
