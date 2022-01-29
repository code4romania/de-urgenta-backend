using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Domain.Api.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class GetCategoryBackpackItemsResponseExample : IExamplesProvider<IImmutableList<BackpackItemModel>>
    {
        public IImmutableList<BackpackItemModel> GetExamples()
        {
            return new List<BackpackItemModel>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Hering conserva",
                    Amount = 1,
                    ExpirationDate = DateTime.Today.AddDays(20),
                    Category = BackpackItemCategoryType.WaterAndFood
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Naut",
                    Amount = 4,
                    ExpirationDate = DateTime.Today.AddDays(365),
                    Category = BackpackItemCategoryType.WaterAndFood
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Coca Cola cu numele tau pe sticla",
                    Amount = 10,
                    ExpirationDate = DateTime.Today.AddDays(420),
                    Category = BackpackItemCategoryType.WaterAndFood
                }
            }.ToImmutableList();
        }
    }
}