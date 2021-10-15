using System;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Domain.Api.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class AddOrUpdateBackpackItemRequestExample : IExamplesProvider<BackpackItemModel>
    {
        public BackpackItemModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = "Hering conserva",
                Amount = 5,
                ExpirationDate = DateTime.Today.AddDays(720),
                CategoryType = BackpackCategoryType.WaterAndFood
            };
        }
    }
}