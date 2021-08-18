using System;
using DeUrgenta.Certifications.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Certifications.Api.Swagger
{
    public class AddOrUpdateCertificationResponseExample : IExamplesProvider<CertificationModel>
    {
        public CertificationModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = "Curs prim ajutor",
                IssuingAuthority = "Crucea Rosie Romania",
                ExpirationDate = DateTime.Today.AddDays(360),
            };
        }
    }
}