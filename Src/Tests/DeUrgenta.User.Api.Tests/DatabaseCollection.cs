using DeUrgenta.Tests.Helpers;
using Xunit;

namespace DeUrgenta.User.Api.Tests
{
    /* Unfortunately we have to have this empty class in each test project due to this issue:
     https://github.com/xunit/xunit/issues/409
    */
    [CollectionDefinition(TestsConstants.DbCollectionName)]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}