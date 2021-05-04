using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class RemoveContributorValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveContributorValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
    }
}