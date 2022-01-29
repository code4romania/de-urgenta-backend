using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Domain.Api;

namespace DeUrgenta.Certifications.Api.QueryHandlers
{
    public class GetCertificationsHandler : IRequestHandler<GetCertifications, Result<IImmutableList<CertificationModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetCertifications> _validator;
        private readonly DeUrgentaContext _context;
        private readonly IBlobStorage _storage;

        public GetCertificationsHandler(IValidateRequest<GetCertifications> validator, DeUrgentaContext context, IBlobStorage storage)
        {
            _validator = validator;
            _context = context;
            _storage = storage;
        }

        public async Task<Result<IImmutableList<CertificationModel>, ValidationResult>> Handle(GetCertifications request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var certifications = await _context.Certifications
            .Where(x => x.User.Sub == request.UserSub)
            .Select(x => new CertificationModel
            {
                Id = x.Id,
                Name = x.Name,
                IssuingAuthority = x.IssuingAuthority,
                ExpirationDate = x.ExpirationDate,
                PhotoUrl = _storage.GetAttachment(x.Id, request.UserSub)
            })
            .ToListAsync(cancellationToken);

            return certifications.ToImmutableList();
        }
    }
}