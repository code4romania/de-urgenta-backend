using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Content.Api.Swagger.Content
{
    public class GetAvailableContentResponseExample :
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