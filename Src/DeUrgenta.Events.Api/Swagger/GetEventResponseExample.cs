using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Common.Models.Events;

namespace DeUrgenta.Events.Api.Swagger
{
    public class GetEventResponseExample : IExamplesProvider<IImmutableList<EventResponseModel>>
    {
        public IImmutableList<EventResponseModel> GetExamples()
        {
            var courses = new List<EventResponseModel>
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
                    IsArchived = false,
                    EventType = "Curs prim ajutor"
                }
            };

            return courses.ToImmutableList();
        }
    }
}