using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackContributorsValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
    }
}