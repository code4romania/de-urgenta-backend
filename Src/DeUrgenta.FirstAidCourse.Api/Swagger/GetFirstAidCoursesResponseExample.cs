using DeUrgenta.FirstAidCourse.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.FirstAidCourse.Api.Swagger
{
    public class GetFirstAidCoursesResponseExample : IExamplesProvider<IImmutableList<FirstAidCourseModel>>
    {
        public IImmutableList<FirstAidCourseModel> GetExamples()
        {
            var certifications = new List<FirstAidCourseModel>
            {
                new()
                {
                    Id = Guid.Parse("64d73b37-ab49-4373-b797-2abec9f3c7d7"),
                    Name = "FA certification 1",
                    IssuingAuthority = "Crucea Rosie Romania",
                    ExpirationDate = new DateTime(2022, 11, 10)
                },
                new()
                {
                    Id = Guid.Parse("8dae4066-f859-456e-9519-fd1d34c81921"),
                    Name = "FA certification 2",
                    IssuingAuthority = "Crucea Rosie Romania",
                    ExpirationDate = DateTime.Today
                },
                new()
                {
                    Id = Guid.Parse("2c06811b-2130-4b7e-abf4-6e78be879ba2"),
                    Name = "FA certification 3",
                    IssuingAuthority = "Bee Gees Saying Alive Foundation",
                    ExpirationDate = DateTime.Today.AddDays(10)
                }
            };

            return certifications.ToImmutableList();
        }
    }
}