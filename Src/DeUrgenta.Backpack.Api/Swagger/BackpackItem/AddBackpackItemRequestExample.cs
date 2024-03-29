﻿using System;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Domain.Api.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class AddBackpackItemRequestExample : IExamplesProvider<BackpackItemRequest>
    {
        public BackpackItemRequest GetExamples()
        {
            return new()
            {
                Name = "Hering conserva",
                Amount = 5,
                ExpirationDate = DateTime.Today.AddDays(720),
                Category = BackpackItemCategoryType.WaterAndFood
            };
        }
    }
}