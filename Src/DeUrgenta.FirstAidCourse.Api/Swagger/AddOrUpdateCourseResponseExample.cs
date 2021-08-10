using System;
using DeUrgenta.Courses.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Courses.Api.Swagger
{
    public class AddOrUpdateCourseResponseExample : IExamplesProvider<CourseResponse>
    {
        public CourseResponse GetExamples()
        {
            return new()
            {
                Name = "Curs prim ajutor",
            };
        }
    }
}