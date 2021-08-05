using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Models;
using MediatR;
using System;

namespace DeUrgenta.FirstAidCourse.Api.Commands
{
    public class CreateFirstAidCourse : IRequest<Result<FirstAidCourseModel>>
    {
        public string UserSub{ get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }
        public string IssuingAuthority { get; }

        public CreateFirstAidCourse(string userSub, FirstAidCourseRequest firstAidCourseRequest)
        {
            UserSub = userSub;
            Name = firstAidCourseRequest.Name;
            ExpirationDate = firstAidCourseRequest.ExpirationDate;
            IssuingAuthority = firstAidCourseRequest.IssuingAuthority;
        }
    }
}