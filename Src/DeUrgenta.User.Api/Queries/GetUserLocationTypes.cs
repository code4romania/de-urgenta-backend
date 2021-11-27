using System.Collections.Immutable;
using DeUrgenta.Common.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetUserLocationTypes : IRequest<IImmutableList<IndexedItemModel>>
    {

    }
}