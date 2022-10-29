using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.CommandHandlers
{
    public class DeleteCertificationHandler : IRequestHandler<DeleteCertification, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteCertification> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteCertificationHandler(IValidateRequest<DeleteCertification> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<Unit, ValidationResult>> Handle(DeleteCertification request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var certification = await _context.Certifications.FirstAsync(c => c.Id == request.CertificationId, ct);
            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }

    }
}
