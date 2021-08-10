using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Courses.Api.Commands
{
    public class DeleteCourse : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid courseId { get; }
        public DeleteCourse(string sub, Guid courseId)
        {
            UserSub = sub;
            this.courseId = courseId;
        }
    }
}
