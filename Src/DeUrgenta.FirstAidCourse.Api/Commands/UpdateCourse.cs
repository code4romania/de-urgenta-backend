using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Models;
using MediatR;

namespace DeUrgenta.Courses.Api.Commands
{
    public class UpdateCourse : IRequest<Result<CourseModel>>
    {
        public string UserSub { get; }
        public Guid FirstAidCourseId { get; }
        public CourseRequest FirstAidCourse { get; }
        public UpdateCourse(string sub, Guid firstAidCourseId, CourseRequest firstAidCourse)
        {
            UserSub = sub;
            FirstAidCourseId = firstAidCourseId;
            FirstAidCourse = firstAidCourse;
        }
    }
}
