using DeUrgenta.Events.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Events.Api.Swagger
{
    public class GetEventCitiesResponseExample : IExamplesProvider<IImmutableList<EventCityModel>>
    {
        public IImmutableList<EventCityModel> GetExamples()
        {
            var cities = new List<EventCityModel>
            {
                new()
                {
                    Name = "Bucuresti",
                },
                new()
                {
                    Name = "Cluj-Napoca",
                },
                new()
                {
                    Name = "Iasi",
                }
            };

            return cities.ToImmutableList();
        }
    }
}