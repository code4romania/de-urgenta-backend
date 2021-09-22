using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using MediatR;
using DeUrgenta.Common.Models;

namespace DeUrgenta.Admin.Api.Queries
{
    public class GetEvents : IRequest<Result<IImmutableList<EventModel>>>
    {
    }
}
