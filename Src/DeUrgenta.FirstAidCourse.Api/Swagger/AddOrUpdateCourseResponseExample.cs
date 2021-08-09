using System;
using DeUrgenta.Courses.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Courses.Api.Swagger
{
    public class AddOrUpdateCourseResponseExample : IExamplesProvider<CourseModel>
    {
        public CourseModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = "Curs prim ajutor",
                IssuingAuthority = "Crucea Rosie Romania",
                ExpirationDate = DateTime.Today.AddDays(360),
            };
        }
    }
}