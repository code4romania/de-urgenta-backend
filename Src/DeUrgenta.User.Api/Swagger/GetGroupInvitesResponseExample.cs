using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class GetGroupInvitesResponseExample : IExamplesProvider<IImmutableList<GropInviteModel>>
    {
        public IImmutableList<GropInviteModel> GetExamples()
        {
            return new List<GropInviteModel>
            {
                new()
                {
                    GroupId = Guid.NewGuid(),
                    GroupName = "Queen band",
                    InviteId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid(),
                    SenderFirstName = "Freddie",
                    SenderLastName = "Mercury"
                },
                new()
                {
                    GroupId = Guid.NewGuid(),
                    GroupName = "Travel lovers",
                    InviteId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid(),
                    SenderFirstName = "Marco",
                    SenderLastName = "Polo"
                }
            }.ToImmutableList();
        }
    }
}