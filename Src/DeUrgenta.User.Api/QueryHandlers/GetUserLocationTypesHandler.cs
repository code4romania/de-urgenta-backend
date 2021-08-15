using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Infra.Models;
using DeUrgenta.User.Api.Queries;
using MediatR;

namespace DeUrgenta.User.Api.QueryHandlers
{
    public class GetUserLocationTypesHandler : IRequestHandler<GetUserLocationTypes, IImmutableList<IndexedItemModel>>
    {
        public Task<IImmutableList<IndexedItemModel>> Handle(GetUserLocationTypes request, CancellationToken cancellationToken)
        {
            var locationTypes = new List<IndexedItemModel>
            {
                new()
                {
                    Id = (int)UserLocationType.Other,
                    Label = "Altă adresă"
                },
                new()
                {
                    Id = (int)UserLocationType.Home,
                    Label = "Casă"
                },
                new()
                {
                    Id = (int)UserLocationType.Work,
                    Label = "Serviciu"
                },
                new()
                {
                    Id = (int)UserLocationType.School,
                    Label = "Școală"
                },
                new()
                {
                    Id = (int)UserLocationType.Gym,
                    Label = "Sala de sport"
                }
            }.ToImmutableList();

            return Task.FromResult<IImmutableList<IndexedItemModel>>(locationTypes);
        }
    }
}