using DeUrgenta.Courses.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Courses.Api.Swagger
{
    public class GetCourseCitiesResponseExample : IExamplesProvider<IImmutableList<CourseCityModel>>
    {
        public IImmutableList<CourseCityModel> GetExamples()
        {
            var cities = new List<CourseCityModel>
            {
                new()
                {
                    Id = 1,
                    Name = "Bucuresti",
                },
                new()
                {
                    Id = 2,
                    Name = "Cluj-Napoca",
                },
                new()
                {
                    Id = 3,
                    Name = "Iasi",
                }
            };

            return cities.ToImmutableList();
        }
    }
}