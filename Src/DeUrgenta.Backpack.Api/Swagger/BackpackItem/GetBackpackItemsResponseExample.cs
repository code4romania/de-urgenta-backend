using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Domain.Api.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class GetBackpackItemsResponseExample : IExamplesProvider<IImmutableList<BackpackItemModel>>
    {
        public IImmutableList<BackpackItemModel> GetExamples()
        {
            var backpackId = Guid.NewGuid();

            return new List<BackpackItemModel>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    BackpackId = backpackId,
                    Name = "Hering conserva",
                    Amount = 1,
                    ExpirationDate = DateTime.Today.AddDays(20),
                    Category = BackpackItemCategoryType.WaterAndFood,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BackpackId = backpackId,
                    Name = "Naut",
                    Amount = 4,
                    ExpirationDate = DateTime.Today.AddDays(365),
                    Category = BackpackItemCategoryType.WaterAndFood
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BackpackId = backpackId,
                    Name = "Coca Cola cu numele tau pe sticla",
                    Amount = 10,
                    ExpirationDate = DateTime.Today.AddDays(420),
                    Category = BackpackItemCategoryType.WaterAndFood
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BackpackId = backpackId,
                    Name = "Topor",
                    Amount = 1,
                    ExpirationDate = DateTime.Today.AddDays(420),
                    Category = BackpackItemCategoryType.SurvivingArticles
                }
            }.ToImmutableList();
        }
    }
}