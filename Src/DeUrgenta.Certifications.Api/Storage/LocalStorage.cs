using System;
using System.IO;
using System.Linq;
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

        public async Task<string> SaveAttachmentAsync(Guid certificationId, string userSub, Stream attachment, string extension)
        {
            var filePath = $"{userSub}/{certificationId}{extension}";

            var userDirectoryPath = Path.Combine(_config.Path, userSub);
            if (!Directory.Exists(userDirectoryPath))
            {
                Directory.CreateDirectory(userDirectoryPath);
            }

            var existingPhotos = Directory.EnumerateFiles(userDirectoryPath, $"{certificationId}.*");
            foreach (string existingPhoto in existingPhotos)
            {
                File.Delete(existingPhoto);
            }

            using (var targetStream = File.Create(Path.Combine(_config.Path, filePath)))
            {
                attachment.Seek(0, SeekOrigin.Begin);
                await attachment.CopyToAsync(targetStream);
            }

            return $"{_config.StaticFilesRequestPath}/{filePath}";
        }

        public string GetAttachment(Guid certificationId, string userSub)
        {
            var userDirectoryPath = Path.Combine(_config.Path, userSub);

            var foundFile = Directory.EnumerateFiles(userDirectoryPath, $"{certificationId}.*").FirstOrDefault();

            return foundFile != null ? $"{_config.StaticFilesRequestPath}/{userSub}/{Path.GetFileName(foundFile)}" : null;
        }
    }
}