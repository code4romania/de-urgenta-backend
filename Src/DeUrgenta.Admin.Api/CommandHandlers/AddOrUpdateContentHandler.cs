using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.I18n.Service.Providers;
using MediatR;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class AddOrUpdateContentHandler : IRequestHandler<AddOrUpdateContent, Result<StringResourceModel, ValidationResult>>
    {
        private readonly IAmContentProvider _contentProvider;
        private readonly IValidateRequest<AddOrUpdateContent> _validator;

        public AddOrUpdateContentHandler(IAmContentProvider contentProvider, IValidateRequest<AddOrUpdateContent> validator)
        {
            _contentProvider = contentProvider;
            _validator = validator;
        }

        public async Task<Result<StringResourceModel, ValidationResult>> Handle(AddOrUpdateContent request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var updatedContent = await _contentProvider.AddOrUpdateContentValue(request.Culture, request.Key, request.Value, ct);

            return updatedContent;
        }
    }
}