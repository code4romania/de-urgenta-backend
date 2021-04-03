using DeUrgenta.Certifications.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DeUrgenta.Certifications.Api.Swagger
{
    public class AddOrUpdateCertificationRequestExample : IExamplesProvider<CertificationRequest>
    {
        public CertificationRequest GetExamples()
        {
            return new()
            {
                Name = "Curs prim ajutor",
                IssuingAuthority = "Crucea Rosie Romania",
                ExpirationDate = DateTime.Today.AddDays(365)
            };
        }
    }
}
