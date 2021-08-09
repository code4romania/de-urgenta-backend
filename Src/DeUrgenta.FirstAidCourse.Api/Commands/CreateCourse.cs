using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Models;
using MediatR;
using System;

namespace DeUrgenta.Courses.Api.Commands
{
    public enum CourseType
    {
        BasicFirstAid,
        QualifiedFirstAid,
        DisasterPrepare
    }

    public class CreateCourse : IRequest<Result<CourseModel>>
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

        public CreateCourse(string userSub, CourseRequest firstAidCourseRequest)
        {
            UserSub = userSub;
            Name = firstAidCourseRequest.Name;
            ExpirationDate = firstAidCourseRequest.ExpirationDate;
            IssuingAuthority = firstAidCourseRequest.IssuingAuthority;
        }
    }
}