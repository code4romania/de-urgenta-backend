using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Events.Api.Models
{
    public sealed record EventModelRequest
    {
        [FromQuery]
        public string City { get; set; }
        [FromQuery]
        public int? EventTypeId { get; set; }
    }
}
