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
using System;

namespace DeUrgenta.Events.Api.QueryHandlers
{
    public class GetEventCitiesHandler : IRequestHandler<GetEventCities, Result<IImmutableList<EventCityModel>>>
    {
        private readonly IValidateRequest<GetEventCities> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventCitiesHandler(IValidateRequest<GetEventCities> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<EventCityModel>>> Handle(GetEventCities request,
            CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<EventCityModel>>("Validation failed");
            }

            var x = await _context.Events
                                .Where(x => (request.EventTypeId == null || x.EventTypeId == request.EventTypeId.Value) && x.OccursOn > DateTime.Today)
                                .Select(x => x.City)
                                .Distinct()
                                .Select(x => new EventCityModel { Name = x })
                                .OrderBy(x => x.Name)
                                .ToListAsync(cancellationToken);
            return x.ToImmutableList();

        }
    }
}