using CSharpFunctionalExtensions;
using DeUrgenta.Events.Api.Models;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using DeUrgenta.Domain.Api;

namespace DeUrgenta.Events.Api.QueryHandlers
{
    public class GetEventCitiesHandler : IRequestHandler<GetEventCities, Result<IImmutableList<EventCityModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetEventCities> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventCitiesHandler(IValidateRequest<GetEventCities> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<EventCityModel>, ValidationResult>> Handle(GetEventCities request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var events = await _context.Events
                                .Where(x => (request.EventTypeId == null || x.EventTypeId == request.EventTypeId.Value) && x.OccursOn > DateTime.Today)
                                .Select(x => x.City)
                                .Distinct()
                                .Select(x => new EventCityModel { Name = x })
                                .OrderBy(x => x.Name)
                                .ToListAsync(cancellationToken);

            return events.ToImmutableList();

        }
    }
}