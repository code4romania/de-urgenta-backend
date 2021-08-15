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
                    Id = Guid.Parse("64d73b37-ab49-4373-b797-2abec9f3c7d7"),
                    Name = "Prim ajutor",
                },
                new()
                {
                    Id = Guid.Parse("8dae4066-f859-456e-9519-fd1d34c81921"),
                    Name = "Prim ajutor calificat",
                },
                new()
                {
                    Id = Guid.Parse("2c06811b-2130-4b7e-abf4-6e78be879ba2"),
                    Name = "Pregatire in caz de dezastre",
                }
            };

            return courseTypes.ToImmutableList();
        }
    }
}