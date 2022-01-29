using DeUrgenta.I18n.Service.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.AdminLocalization
{
    public class AddOrUpdateContentResponseExample :
    IExamplesProvider<StringResourceModel>
    {
        public StringResourceModel GetExamples()
        {
            return new StringResourceModel
            {
                Key = "terms-and-conditions",
                Value = "Terms and conditions"
            };
        }
    }
}