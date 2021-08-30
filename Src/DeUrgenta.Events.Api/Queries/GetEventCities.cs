using System;
using DeUrgenta.Events.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Events.Api.Queries
{
    public class GetEventCities : IRequest<Result<IImmutableList<EventCityModel>>>
    {
        public int? EventTypeId { get; }

        public GetEventCities(int? eventTypeId)
        {
            EventTypeId = eventTypeId;
        }
    }
}
