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
                new CertificationModel(1,"FA certification 1", new DateTime(2022,11,10)),
                new CertificationModel(2,"FA certification 2", DateTime.Today),
                new CertificationModel(3,"FA certification 3", DateTime.Today.AddDays(10))
            };

            return certifications.ToImmutableList();
        }

    }
}
