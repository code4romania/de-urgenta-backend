﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class GetBackpackItemsResponseExample : IExamplesProvider<IImmutableList<BackpackItemModel>>
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
                    ExpirationDate = DateTime.Today.AddDays(20)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Naut",
                    Amount = 4,
                    ExpirationDate = DateTime.Today.AddDays(365)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Coca Cola cu numele tau pe sticla",
                    Amount = 10,
                    ExpirationDate = DateTime.Today.AddDays(420)
                }
            }.ToImmutableList();
        }
    }
}