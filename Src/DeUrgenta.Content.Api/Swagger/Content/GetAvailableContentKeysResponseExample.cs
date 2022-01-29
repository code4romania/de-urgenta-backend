using System.Collections.Generic;
using System.Collections.Immutable;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Content.Api.Swagger.Content
{
    public class GetAvailableContentKeysResponseExample : IExamplesProvider<ImmutableList<string>>
    {
        public ImmutableList<string> GetExamples()
        {
            return new List<string>{
                "terms-and-conditions",
                "header-message"
            }.ToImmutableList();
        }
    }
}