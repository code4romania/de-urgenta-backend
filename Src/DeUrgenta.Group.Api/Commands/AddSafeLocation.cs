﻿using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class AddSafeLocation : IRequest<Result<SafeLocationResponseModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }
        public SafeLocationRequest SafeLocation { get; }


        public AddSafeLocation(string userSub, Guid groupId, SafeLocationRequest safeLocation)
        {
            UserSub = userSub;
            GroupId = groupId;
            SafeLocation = safeLocation;
        }
    }
}