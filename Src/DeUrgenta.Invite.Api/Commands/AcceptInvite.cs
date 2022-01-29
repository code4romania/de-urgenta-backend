﻿using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptInvite : IRequest<Result<AcceptInviteModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid InviteId { get; }

        public AcceptInvite(string sub, Guid inviteId)
        {
            UserSub = sub;
            InviteId = inviteId;
        }
    }
}