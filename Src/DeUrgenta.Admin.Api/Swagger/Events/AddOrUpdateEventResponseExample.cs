using System;
using DeUrgenta.Common.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.Events
{
    public class AddOrUpdateEventResponseExample : IExamplesProvider<EventModel>
    {
        public EventModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Author = "Albert Einstein",
                ContentBody = "<h1> E = mc^2 ajutor.</h1>",
                Title = "Curs relativ prim ajutor",
                OccursOn = DateTime.Today.AddDays(30),
                OrganizedBy = "Crucea rosie"
            };
        }
    }
}