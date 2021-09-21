using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.QueryHandlers;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Common.Validation;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.QueryHandlers
{
    public class GetCertificationPhotoHandlerShould
    {
        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            //Arrange 
            var storage = Substitute.For<IBlobStorage>();
            var validator = Substitute.For<IValidateRequest<GetCertificationPhoto>>();
            validator
                .IsValidAsync(Arg.Any<GetCertificationPhoto>())
                .Returns(Task.FromResult(false));

            var request = new GetCertificationPhoto("user-sub", Guid.NewGuid());
            var sut = new GetCertificationPhotoHandler(validator, storage);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeFalse();
        }
    }
}
