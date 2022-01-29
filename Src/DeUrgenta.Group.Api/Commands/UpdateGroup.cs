﻿using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class UpdateGroup : IRequest<Result<GroupModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }
        public GroupRequest Group { get; }

        public UpdateGroup(string userSub, Guid groupId, GroupRequest group)
        {
            UserSub = userSub;
            GroupId = groupId;
            Group = group;
        }
    }
}