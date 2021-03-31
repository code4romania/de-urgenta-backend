using DeUrgenta.Certifications.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DeUrgenta.Certifications.Api.Swagger
{
    public class AddNewCertificationModelExample : IExamplesProvider<NewCertificationModel>
    {
        public NewCertificationModel GetExamples()
        {
            return new()
            {
                Name = "New FA certificate",
                ExpirationDate = DateTime.Today.AddDays(365)
            };
        }
    }
}
