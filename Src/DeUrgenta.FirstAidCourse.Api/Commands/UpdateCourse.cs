using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Models;
using MediatR;

namespace DeUrgenta.Courses.Api.Commands
{
    public class UpdateCourse : IRequest<Result<CourseTypeModel>>
    {
        public string UserSub { get; }
        public Guid CourseId { get; }
        public CourseRequest Course { get; }
        public UpdateCourse(string sub, Guid courseId, CourseRequest course)
        {
            UserSub = sub;
            CourseId = courseId;
            Course = course;
        }
    }
}
