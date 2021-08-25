using DeUrgenta.Courses.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Courses.Api.Swagger
{
    public class GetCourseTypesResponseExample : IExamplesProvider<IImmutableList<CourseTypeModel>>
    {
        public IImmutableList<CourseTypeModel> GetExamples()
        {
            var courseTypes = new List<CourseTypeModel>
            {
                new()
                {
                    Id = 1,
                    Name = "Prim ajutor",
                },
                new()
                {
                    Id = 2,
                    Name = "Prim ajutor calificat",
                },
                new()
                {
                    Id = 3,
                    Name = "Pregatire in caz de dezastre",
                }
            };

            return courseTypes.ToImmutableList();
        }
    }
}