using System;
using System.IO;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.Storage
{
    public interface IBlobStorage //TODO add ct support to methods
    {
        Task<string> SaveAttachmentAsync(Guid certificationId, string userSub, Stream attachment, string extension);

        string GetAttachment(Guid certificationId, string userSub);
    }
}
