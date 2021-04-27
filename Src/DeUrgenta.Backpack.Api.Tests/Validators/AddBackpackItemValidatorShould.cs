using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection("Database collection")]
    public class AddBackpackItemValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        public AddBackpackItemValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_when_no_backpack_found_by_id()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);
            var command = new AddBackpackItem(Guid.NewGuid(), new BackpackItemRequest());

            // Act
            bool isValid = await sut.IsValidAsync(command);

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_backpack_found_by_id()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);
            var backpackId = Guid.NewGuid();
            await _dbContext.Backpacks.AddAsync(new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "test_backpack"
            });

            await _dbContext.SaveChangesAsync();

            var command = new AddBackpackItem(backpackId, new BackpackItemRequest());

            // Act
            bool isValid = await sut.IsValidAsync(command);

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
