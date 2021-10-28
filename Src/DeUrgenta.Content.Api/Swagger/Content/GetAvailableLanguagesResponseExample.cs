using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Content.Api.Swagger.Content
{
    public class GetAvailableLanguagesResponseExample : IExamplesProvider<IImmutableList<LanguageModel>>
    {
        public IImmutableList<LanguageModel> GetExamples()
        {
            return new List<LanguageModel>{
                new LanguageModel{
                    Id = new Guid(),
                    Name = "English",
                    Culture = "en-US"
                },
                new LanguageModel{
                    Id = new Guid(),
                    Name = "Romanian",
                    Culture = "ro-RO"
                },
                new LanguageModel{
                    Id = new Guid(),
                    Name = "Hungarian",
                    Culture = "hu-HU"
                }
            }.ToImmutableList();
        }
    }
}