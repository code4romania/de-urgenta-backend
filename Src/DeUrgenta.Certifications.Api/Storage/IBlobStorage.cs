using System;
using System.IO;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.Storage
{
    public interface IBlobStorage
    {
        Task<string> SaveAttachment(Guid certificationId, Stream attachment);

        string GetAttachment(Guid certificationId);
    }
}
