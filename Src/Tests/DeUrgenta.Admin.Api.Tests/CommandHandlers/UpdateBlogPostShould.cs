using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.CommandHandlers;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBlogPostShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateBlogPostShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateBlogPost>>();
            validator
                .IsValidAsync(Arg.Any<UpdateBlogPost>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new UpdateBlogPostHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateBlogPost(Guid.NewGuid(), new BlogPostRequest
            {
                Author = "Test", Title = "Test", ContentBody = "<h1>Test</h1>"
            }), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}