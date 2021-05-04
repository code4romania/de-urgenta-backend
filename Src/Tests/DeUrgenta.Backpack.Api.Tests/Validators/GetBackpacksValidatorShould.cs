using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpacksValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpacksValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
    }
}