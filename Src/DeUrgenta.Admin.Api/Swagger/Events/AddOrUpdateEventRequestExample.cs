using System;
using DeUrgenta.Admin.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.Events
{
    public class AddOrUpdateEventRequestExample : IExamplesProvider<EventRequest>
    {
        public EventRequest GetExamples()
        {
            return new()
            {
              Author = "Albert Einstein",
              ContentBody = "<h1> E = mc^2 ajutor.</h1>",
              Title = "Curs relativ prim ajutor",
              OccursOn = DateTime.Today.AddDays(30),
              OrganizedBy = "Crucea rosie",
              City = "Suceava",
              EventTypeId = 1,
              Address = "some address"
            };
        }
    }
}
