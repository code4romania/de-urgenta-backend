using System;

namespace DeUrgenta.Courses.Api.Models
{
    public sealed record EventModelRequest
    {
        public int? CityId { get; set; }
        public int? EventTypeId { get; set; }
    }
}
