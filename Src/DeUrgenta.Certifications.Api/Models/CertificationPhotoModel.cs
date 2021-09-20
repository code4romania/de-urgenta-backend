namespace DeUrgenta.Certifications.Api.Models
{
    public sealed record CertificationPhotoModel
    {
        public string Title { get; set; }
        public byte[] Photo { get; set; }
    }
}