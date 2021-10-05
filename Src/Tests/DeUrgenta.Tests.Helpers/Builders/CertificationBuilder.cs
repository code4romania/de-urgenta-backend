using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class CertificationBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private DateTime _expirationDate = DateTime.Today.AddDays(3);

        public Certification Build()
            => new()
            {
                Name = TestDataProviders.RandomString(),
                IssuingAuthority = TestDataProviders.RandomString(),
                ExpirationDate = _expirationDate,
                Id = Guid.NewGuid(),
                UserId = _userId
            };

        public CertificationBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;

        }

        public CertificationBuilder WithExpirationDate(DateTime expirationDate)
        {
            _expirationDate = expirationDate;
            return this;
        }
    }
}
