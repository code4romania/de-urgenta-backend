using System;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger.User
{
    public class AddUserLocationResponseExample : IExamplesProvider<UserLocationModel>
    {
        public UserLocationModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Address = "Splaiul Unirii 160, 040041 Bucharest, Romania",
                Type = UserLocationType.Work,
                Longitude = 44.41184746891321m,
                Latitude = 26.118310383230373m
            };
        }
    }
}