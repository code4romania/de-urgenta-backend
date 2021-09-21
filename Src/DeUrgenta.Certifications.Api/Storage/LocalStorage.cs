using System;
using System.IO;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Storage.Config;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Certifications.Api.Storage
{
    public class LocalStorage : IBlobStorage
    {
        private readonly LocalConfigOptions _config;

        public LocalStorage(IOptionsMonitor<LocalConfigOptions> config)
        {
            _config = config.CurrentValue;
        }

        public async Task<string> SaveAttachmentAsync(Guid certificationId, string userSub, Stream attachment)
        {
            var filePath = $"{userSub}/{certificationId}";

            using (var targetStream = File.Create(Path.Combine(_config.Path, filePath)))
            {
                await attachment.CopyToAsync(targetStream);
            }

            return filePath;
        }

        public string GetAttachment(Guid certificationId, string userSub)
        {
            var filePath = $"{userSub}/{certificationId}";

            return filePath;
        }
    }
}