using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DeUrgenta.Common.Validation
{
    public class ImageFileValidator : AbstractValidator<IFormFile>
    {
        private const long FiveMegabytes = 5242880;
        private const int MaxFileNameLength = 250;

        public ImageFileValidator()
        {
            RuleFor(f => f.FileName).Length(0, MaxFileNameLength)
                .WithMessage($"File name must not be longer than {MaxFileNameLength} characters.");

            RuleFor(f => f.Length).InclusiveBetween(0, FiveMegabytes)
                .WithMessage("File size is larger than 5 MB.");

            RuleFor(f => f.ContentType)
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("File must be an image.");
        }

    }
}
