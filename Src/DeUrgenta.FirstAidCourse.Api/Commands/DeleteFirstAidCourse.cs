using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.FirstAidCourse.Api.Commands
{
    public class DeleteFirstAidCourse : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid FirstAidCourseId { get; }
        public DeleteFirstAidCourse(string sub, Guid firstAidCourseId)
        {
            UserSub = sub;
            FirstAidCourseId = firstAidCourseId;
        }
    }
}
