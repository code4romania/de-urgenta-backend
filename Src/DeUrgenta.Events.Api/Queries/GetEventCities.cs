using DeUrgenta.Events.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Events.Api.Queries
{
    public class GetEventCities : IRequest<Result<IImmutableList<EventCityModel>, ValidationResult>>
    {
        public int? EventTypeId { get; }

        public GetEventCities(int? eventTypeId)
        {
            EventTypeId = eventTypeId;
        }
    }
}
