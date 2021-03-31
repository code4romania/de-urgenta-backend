using DeUrgenta.Certifications.Api.Models;
using MediatR;
using System.Collections.Immutable;

namespace DeUrgenta.Certifications.Api.Queries
{
    public class GetCertifications : IRequest<IImmutableList<CertificationModel>>
    {
        public int UserId { get; }

        public GetCertifications(int userId)
        {
            UserId = userId;
        }
    }
}
