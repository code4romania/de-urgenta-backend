using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class InviteToBackpackContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public InviteToBackpackContributorsValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
    }
}