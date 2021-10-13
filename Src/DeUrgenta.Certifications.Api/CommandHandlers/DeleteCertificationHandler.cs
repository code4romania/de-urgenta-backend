using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
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
        public async Task<Result<Unit, ValidationResult>> Handle(DeleteCertification request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var certification = await _context.Certifications.FirstAsync(c => c.Id == request.CertificationId, cancellationToken);
            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}
