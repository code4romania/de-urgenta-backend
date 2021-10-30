using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.CommandHandlers;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateBlogPostHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public CreateBlogPostHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<CreateBlogPost>>();
            validator
                .IsValidAsync(Arg.Any<CreateBlogPost>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new CreateBlogPostHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new CreateBlogPost(new BlogPostRequest
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "<h1>Test</h1>"
            }), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}