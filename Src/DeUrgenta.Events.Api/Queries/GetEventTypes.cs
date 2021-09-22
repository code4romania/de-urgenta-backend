using DeUrgenta.Events.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Events.Api.Queries
{
    public class GetEventTypes : IRequest<Result<IImmutableList<EventTypeModel>>>
    {
    }
}
