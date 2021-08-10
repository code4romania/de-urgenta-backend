using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Domain.Entities;
using MediatR;
using System;

namespace DeUrgenta.Courses.Api.Commands
{
    public class CreateCourse : IRequest<Result<CourseTypeModel>>
    {
        public CourseType CourseType { get; }
        public string City { get; }


        public string UserSub { get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }
        public string IssuingAuthority { get; }

        //public DateTime CourseDate { get; }
        //public string Address { get; }
        //public string OrganizerName { get; }
        //public string OrganizerDetails { get; }

        public CreateCourse(string userSub, CourseRequest courseRequest)
        {
            UserSub = userSub;
            Name = courseRequest.Name;
            ExpirationDate = courseRequest.ExpirationDate;
            IssuingAuthority = courseRequest.IssuingAuthority;
        }
    }
}