using DeUrgenta.FirstAidCourse.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DeUrgenta.FirstAidCourse.Api.Swagger
{
    public class AddOrUpdateFirstAidCourseRequestExample : IExamplesProvider<FirstAidCourseRequest>
    {
        public FirstAidCourseRequest GetExamples()
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
