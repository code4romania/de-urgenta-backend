using DeUrgenta.Events.Api.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Events.Api.Swagger
{
    public class GetEventTypesResponseExample : IExamplesProvider<IImmutableList<EventTypeModel>>
    {
        public IImmutableList<EventTypeModel> GetExamples()
        {
            var courseTypes = new List<EventTypeModel>
            {
                new()
                {
                    Id = 1,
                    Name = "Prim ajutor",
                },
                new()
                {
                    Id = 2,
                    Name = "Prim ajutor calificat",
                },
                new()
                {
                    Id = 3,
                    Name = "Pregatire in caz de dezastre",
                }
            };

            return courseTypes.ToImmutableList();
        }
    }
}