using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class GetBackpackInvitesResponseExample : IExamplesProvider<IImmutableList<BackpackInviteModel>>
    {
        public IImmutableList<BackpackInviteModel> GetExamples()
        {
            return new List<BackpackInviteModel>
            {
                new()
                {
                    BackpackId = Guid.NewGuid(),
                    BackpackName = "Turbinca",
                    InviteId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid(),
                    SenderFirstName = "Ivan",
                    SenderLastName = "Turbinca"
                },
                new()
                {
                    BackpackId = Guid.NewGuid(),
                    BackpackName = "Big red bag",
                    InviteId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid(),
                    SenderFirstName = "Santa",
                    SenderLastName = "Klaus"
                }
            }.ToImmutableList();
        }
    }
}