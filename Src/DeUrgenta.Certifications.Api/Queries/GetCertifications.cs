using System;
using DeUrgenta.Certifications.Api.Models;
using MediatR;
using System.Collections.Immutable;

namespace DeUrgenta.Certifications.Api.Queries
{
    public class GetCertifications : IRequest<IImmutableList<CertificationModel>>
    {
        public Guid UserId { get; }

        public GetCertifications(Guid userId)
        {
            UserId = userId;
        }
    }
}
