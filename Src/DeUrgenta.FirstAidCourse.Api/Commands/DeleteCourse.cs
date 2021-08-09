using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Courses.Api.Commands
{
    public class DeleteCourse : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid FirstAidCourseId { get; }
        public DeleteCourse(string sub, Guid firstAidCourseId)
        {
            UserSub = sub;
            FirstAidCourseId = firstAidCourseId;
        }
    }
}
