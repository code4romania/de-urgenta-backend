using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Infra.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger.User
{
    public class GetUserLocationTypesResponseExample : IExamplesProvider<IImmutableList<IndexedItemModel>>
    {
        public IImmutableList<IndexedItemModel> GetExamples()
        {
            return new List<IndexedItemModel>
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
        }
    }
}