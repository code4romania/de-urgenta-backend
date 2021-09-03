using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Models;
using DeUrgenta.Events.Api.Models;
using MediatR;

namespace DeUrgenta.Events.Api.Queries
{
    public class GetEvent : IRequest<Result<IImmutableList<EventModel>>>
    {
        public EventModelRequest ModelRequest { get; }

        public GetEvent(EventModelRequest modelRequest)
        {
            ModelRequest = modelRequest;
        }
    }
}
