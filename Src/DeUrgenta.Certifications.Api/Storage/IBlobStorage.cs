using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.Storage
{
    public interface IBlobStorage
    {
        Task<string> SaveAttachmentAsync(Guid certificationId, string userSub, Stream attachment, string extension, CancellationToken cancellationToken);

        string GetAttachment(Guid certificationId, string userSub);
    }
}
