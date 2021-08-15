using System;
using CSharpFunctionalExtensions;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class UpdateLocation : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid LocationId { get; }
        public UserLocationRequest Location { get; }

        public UpdateLocation(string userSub, Guid locationId, UserLocationRequest location)
        {
            UserSub = userSub;
            LocationId = locationId;
            Location = location;
        }
    }
}