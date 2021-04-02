using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackCategory
{
    public class GetBackpackCategoriesResponseExample : IExamplesProvider<IImmutableList<BackpackCategoryModel>>
    {
        public IImmutableList<BackpackCategoryModel> GetExamples()
        {
            return new List<BackpackCategoryModel>
            {
                new(){Id = Guid.NewGuid(),Name ="Apa si alimente" },
                new(){Id = Guid.NewGuid(),Name ="Articole igiena" },
                new(){Id = Guid.NewGuid(),Name = "Trusa de prim ajutor"},
                new(){Id = Guid.NewGuid(),Name = "Documente"},
                new(){Id = Guid.NewGuid(),Name = "Articole de supravietuire"},
                new(){Id = Guid.NewGuid(),Name = "Diverse"},
            }.ToImmutableList();
        }
    }
}