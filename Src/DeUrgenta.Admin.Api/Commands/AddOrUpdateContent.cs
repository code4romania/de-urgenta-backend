using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Models;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class AddOrUpdateContent : IRequest<Result<StringResourceModel, ValidationResult>>
    {
        public string Culture { get; }
        public string Key { get; }
        public string Value { get; }

        public AddOrUpdateContent(string culture, string key, string value)
        {
            Culture = culture;
            Key = key;
            Value = value;
        }
    }
}