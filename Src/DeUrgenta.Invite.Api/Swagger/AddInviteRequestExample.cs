using System;
using DeUrgenta.Invite.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Invite.Api.Swagger
{
    public class AddInviteRequestExample: IExamplesProvider<InviteRequest>
    {
        public InviteRequest GetExamples()
        {
            return new() { DestinationId = Guid.NewGuid(), Type = InviteType.Group };
        }
    }
}