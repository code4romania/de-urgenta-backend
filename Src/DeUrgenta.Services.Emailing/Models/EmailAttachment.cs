namespace DeUrgenta.Services.Emailing.Models
{
    public class EmailAttachment
    {
        public EmailAttachment(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public string FileName { get; set; }

        public byte[] Content { get; set; }
    }
}