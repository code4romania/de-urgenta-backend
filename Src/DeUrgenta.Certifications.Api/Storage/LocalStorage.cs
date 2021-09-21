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

        public Task<string> SaveAttachment(Guid certificationId, Stream attachment)
        {
            throw new NotImplementedException();
        }

        public string GetAttachment(Guid certificationId)
        {
            throw new NotImplementedException();
        }
    }
}