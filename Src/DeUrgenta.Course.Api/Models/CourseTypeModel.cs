using System;

namespace DeUrgenta.Courses.Api.Models
{
    public sealed record CourseTypeModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
