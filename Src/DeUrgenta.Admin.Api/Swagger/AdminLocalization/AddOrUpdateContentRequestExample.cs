using DeUrgenta.Admin.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.AdminLocalization
{
    public class AddOrUpdateContentRequestExample : IExamplesProvider<AddOrUpdateContentModel>
    {
        public AddOrUpdateContentModel GetExamples()
        {
            return new AddOrUpdateContentModel
            {
                Culture = "en-US",
                Key = "terms-and-conditions",
                Value = "Terms and conditions"
            };
        }
    }
}