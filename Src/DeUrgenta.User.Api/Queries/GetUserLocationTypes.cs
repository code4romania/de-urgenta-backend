using System.Collections.Immutable;
using DeUrgenta.Infra.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetUserLocationTypes : IRequest<IImmutableList<IndexedItemModel>>
    {

    }
}