using System;
using System.IO;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Tests.Helpers;
using Microsoft.AspNetCore.Http;

namespace DeUrgenta.Certifications.Api.Tests.Builders
{
    public class CertificationRequestBuilder
    {
        private readonly string _name = TestDataProviders.RandomString();
        private readonly DateTime _expirationDate = DateTime.Today.AddDays(2);
        private readonly string _issuingAuthority = TestDataProviders.RandomString();
        private IFormFile _photo = new FormFile(Stream.Null, 0, 10, TestDataProviders.RandomString(), TestDataProviders.RandomString());

        public CertificationRequest Build() => new()
        {
            Name = _name,
            ExpirationDate = _expirationDate,
            IssuingAuthority = _issuingAuthority,
            Photo = _photo
        };

        public CertificationRequestBuilder WithPhoto(IFormFile photo)
        {
            _photo = photo;
            return this;
        }
    }
}
