using CSharpFunctionalExtensions;
using DeUrgenta.Events.Api.Models;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.Events.Api.QueryHandlers
{
    public class GetEventTypesHandler : IRequestHandler<GetEventTypes, Result<IImmutableList<EventTypeModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetEventTypes> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventTypesHandler(IValidateRequest<GetEventTypes> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<EventTypeModel>, ValidationResult>> Handle(GetEventTypes request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var courseTypes = await _context.EventTypes
            .Select(x => new EventTypeModel
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);

            return courseTypes.ToImmutableList();
        }
    }
}