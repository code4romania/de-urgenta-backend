using DeUrgenta.Common.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Events.Api.Swagger
{
    public class GetEventsResponseExample : IExamplesProvider<IImmutableList<EventModel>>
    {
        public IImmutableList<EventModel> GetExamples()
        {
            var courses = new List<EventModel>
            {
                new()
                {
                    Id = Guid.Parse("34d73b37-ab49-4373-b797-2abec9f3c7d7"),
                    Address = "Event Address",
                    Author = "Event Organizer",
                    City = "Deva",
                    ContentBody = "Lorem ipsum color",
                    OccursOn = DateTime.Now.AddDays(-100),
                    OrganizedBy = "Event Organizer",
                    PublishedOn = DateTime.Now.AddDays(-50),
                    Title = "Curs de prim ajutor",
                }
            };

            return courses.ToImmutableList();
        }
    }
}