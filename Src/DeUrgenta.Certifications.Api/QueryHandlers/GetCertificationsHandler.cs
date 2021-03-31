using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.QueryHandlers
{
    internal class GetCertificationsHandler : IRequestHandler<GetCertifications, IImmutableList<CertificationModel>>
    {
        private readonly DeUrgentaContext _context;

        public GetCertificationsHandler(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<IImmutableList<CertificationModel>> Handle(GetCertifications request, CancellationToken cancellationToken)
        {
            var certifications = await _context.Certifications
                .Where(x => x.IdUser == request.UserId)
                .Select(x => new CertificationModel(x.Id, x.Name, x.ExpirationDate))
                .ToListAsync(cancellationToken: cancellationToken);

            return certifications.ToImmutableList();
        }
    }
}
