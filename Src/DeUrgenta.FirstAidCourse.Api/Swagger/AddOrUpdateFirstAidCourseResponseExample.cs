using System;
using DeUrgenta.FirstAidCourse.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.FirstAidCourse.Api.Swagger
{
    public class AddOrUpdateFirstAidCourseResponseExample : IExamplesProvider<FirstAidCourseModel>
    {
        public FirstAidCourseModel GetExamples()
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