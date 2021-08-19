using System;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators.RequestValidators;
using DeUrgenta.Domain.Entities;
using Xunit;
using FluentValidation.TestHelper;

namespace DeUrgenta.Backpack.Api.Tests.RequsetValidators
{
    public class BackpackItemRequestValidatorShould
    {
        private readonly BackpackItemRequestValidator _validator;
        public BackpackItemRequestValidatorShould()
        {
            _validator = new BackpackItemRequestValidator();
        }
        
        [Fact]
        public void Invalidate_when_expiration_date_is_in_the_past()
        {
            var model = new BackpackItemRequest
            {
                Name = "Name",
                Amount = 12,
                CategoryType = BackpackCategoryType.FirstAid,
                ExpirationDate = DateTime.Today.AddDays(-1)
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.ExpirationDate);
        }

        [Fact]
        public void Validate_when_expiration_date_is_null()
        {
            var model = new BackpackItemRequest
            {
                Name = "Name",
                Amount = 12,
                CategoryType = BackpackCategoryType.FirstAid,
                ExpirationDate = null
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}