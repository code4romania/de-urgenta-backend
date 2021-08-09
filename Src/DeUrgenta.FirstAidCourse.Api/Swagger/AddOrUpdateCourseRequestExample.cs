using DeUrgenta.Courses.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DeUrgenta.Courses.Api.Swagger
{
    public class AddOrUpdateCourseRequestExample : IExamplesProvider<CourseRequest>
    {
        public CourseRequest GetExamples()
        {
            return new()
            {
                Name = "Curs prim ajutor",
                IssuingAuthority = "Crucea Rosie Romania",
                ExpirationDate = DateTime.Today.AddDays(365)
            };
        }
    }
}
