using System;
using DeUrgenta.Domain.Entities;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class AddUserLocationResponseExample : IExamplesProvider<UserLocationModel>
    {
        public UserLocationModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Address = "Splaiul Unirii 160, 040041 Bucharest, Romania",
                Type = UserAddressType.Work,
                Longitude = 44.41184746891321m,
                Latitude = 26.118310383230373m
            };
        }
    }
}