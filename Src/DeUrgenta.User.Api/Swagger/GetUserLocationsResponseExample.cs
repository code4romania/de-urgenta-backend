using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Domain.Entities;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class GetUserLocationsResponseExample : IExamplesProvider<IImmutableList<UserLocationModel>>
    {
        public IImmutableList<UserLocationModel> GetExamples()
        {
            return new List<UserLocationModel>
            {
                new ()
                {
                    Latitude = 44.42100028019816m,
                    Longitude= 26.137300692423988m,
                    Address = "Bulevardul camil ressu, sector 3 2 A, Bucuresti 011135",
                    Type = UserAddressType.Work,
                    Id = Guid.NewGuid()
                },
                new ()
                {
                    Latitude = 44.42237955141269m,
                    Longitude=  26.16373654467784m,
                    Address = "Bulevardul Nicolae Grigorescu nr. 20 CA14, Bucuresti 030453",
                    Type = UserAddressType.School,
                    Id = Guid.NewGuid()
                },
                new ()
                {
                    Latitude = 44.41964679721312m,
                    Longitude=  26.115253981442685m,
                    Address = "Strada Nerva Traian, Bucuresti",
                    Type = UserAddressType.Home,
                    Id = Guid.NewGuid()
                },
                new ()
                {
                    Latitude = 44.43104805920341m,
                    Longitude= 26.100319441533046m,
                    Address = "Str. Franceza 17, Bucuresti 030167",
                    Type = UserAddressType.Gym,
                    Id = Guid.NewGuid()
                },

            }.ToImmutableList();
        }
    }
}