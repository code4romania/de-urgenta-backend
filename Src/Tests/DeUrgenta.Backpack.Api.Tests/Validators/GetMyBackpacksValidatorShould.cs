using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetMyBackpacksValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetMyBackpacksValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
    }
}