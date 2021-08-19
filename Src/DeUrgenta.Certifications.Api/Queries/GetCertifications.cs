using DeUrgenta.Certifications.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Certifications.Api.Queries
{
    public class GetCertifications : IRequest<Result<IImmutableList<CertificationModel>>>
    {
        public string UserSub { get; }

        public GetCertifications(string userSub)
        {
            UserSub = userSub;
        }
    }
}
