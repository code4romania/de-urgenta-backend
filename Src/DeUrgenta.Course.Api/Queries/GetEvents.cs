using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Models;
using DeUrgenta.Courses.Api.Models;
using MediatR;

namespace DeUrgenta.Courses.Api.Queries
{
    public class GetEvents : IRequest<Result<IImmutableList<EventModel>>>
    {
        public EventModelRequest ModelRequest { get; }

        public GetEvents(EventModelRequest modelRequest)
        {
            ModelRequest = modelRequest;
        }
    }
}
