using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class GroupBuilder
    {
        private User _admin = new User();

        public Group Build() => new Group {Admin = _admin, Name = TestDataProviders.RandomString()};

        public GroupBuilder WithAdmin(User admin)
        {
            _admin = admin;
            return this;
        }
    }
}