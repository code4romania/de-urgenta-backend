using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.QueryHandlers
{
    public class GetGroupMembersHandler : IRequestHandler<GetGroupMembers, Result<IImmutableList<GroupMemberModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetGroupMembers> _validator;
        private readonly DeUrgentaContext _context;

        public GetGroupMembersHandler(IValidateRequest<GetGroupMembers> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<GroupMemberModel>, ValidationResult>> Handle(GetGroupMembers request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var groupMembers = await _context
                .UsersToGroups
                .Where(x => x.GroupId == request.GroupId)
                .Include(x => x.User)
                .Include(x => x.Group)
                .Select(x => new { x.User, x.Group, HasValidCertification = x.User.Certifications.Any(c => c.ExpirationDate.Date > DateTime.Today) })
                .Select(x => new GroupMemberModel
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsGroupAdmin = x.Group.AdminId == x.User.Id,
                    HasValidCertification = x.HasValidCertification
                })
                .ToListAsync(ct);

            return groupMembers.ToImmutableList();
        }
    }
}