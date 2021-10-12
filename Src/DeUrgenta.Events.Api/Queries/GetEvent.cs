using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Validation;
using DeUrgenta.Events.Api.Models;
using MediatR;

namespace DeUrgenta.Events.Api.Queries
{
    public class GetEvent : IRequest<Result<IImmutableList<EventResponseModel>, ValidationResult>>
    {
        public EventModelRequest Filter { get; }

        public GetEvent(EventModelRequest filter)
        {
            Filter = filter;
        }
    }
}
