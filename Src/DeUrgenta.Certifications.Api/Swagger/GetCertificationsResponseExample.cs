using DeUrgenta.Certifications.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Certifications.Api.Swagger
{
    public class GetCertificationsResponseExample : IExamplesProvider<IImmutableList<CertificationModel>>
    {

        public IImmutableList<CertificationModel> GetExamples()
        {
            var certifications = new List<CertificationModel>()
            {
                new CertificationModel(Guid.Parse("64d73b37-ab49-4373-b797-2abec9f3c7d7"),"FA certification 1", new DateTime(2022,11,10)),
                new CertificationModel(Guid.Parse("8dae4066-f859-456e-9519-fd1d34c81921"),"FA certification 2", DateTime.Today),
                new CertificationModel(Guid.Parse("2c06811b-2130-4b7e-abf4-6e78be879ba2"),"FA certification 3", DateTime.Today.AddDays(10))
            };

            return certifications.ToImmutableList();
        }

    }
}
