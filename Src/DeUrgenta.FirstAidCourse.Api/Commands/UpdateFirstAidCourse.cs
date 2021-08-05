using System;
using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Models;
using MediatR;

namespace DeUrgenta.FirstAidCourse.Api.Commands
{
    public class UpdateFirstAidCourse : IRequest<Result<FirstAidCourseModel>>
    {
        public string UserSub { get; }
        public Guid FirstAidCourseId { get; }
        public FirstAidCourseRequest FirstAidCourse { get; }
        public UpdateFirstAidCourse(string sub, Guid firstAidCourseId, FirstAidCourseRequest firstAidCourse)
        {
            UserSub = sub;
            FirstAidCourseId = firstAidCourseId;
            FirstAidCourse = firstAidCourse;
        }
    }
}
