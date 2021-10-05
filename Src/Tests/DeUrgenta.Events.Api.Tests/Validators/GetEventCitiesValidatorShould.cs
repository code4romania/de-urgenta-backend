﻿using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Validators;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventCitiesValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventCitiesValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        public async Task ShouldInvalidateWhenInvalidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventCitiesValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetEventCities(eventTypeId));

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task ShouldValidateWhenValidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventCitiesValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetEventCities(eventTypeId));

            // Assert
            isValid.Should().BeTrue();
        }
    }
}
