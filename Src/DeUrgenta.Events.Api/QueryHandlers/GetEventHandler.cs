using CSharpFunctionalExtensions;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Domain.Api;

namespace DeUrgenta.Events.Api.QueryHandlers
{
    public class GetEventHandler : IRequestHandler<GetEvent, Result<IImmutableList<EventResponseModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetEvent> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventHandler(IValidateRequest<GetEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<EventResponseModel>, ValidationResult>> Handle(GetEvent request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var events = await _context.Events
                                            .Where(x => request.Filter.EventTypeId == null || x.EventType.Id == request.Filter.EventTypeId.Value)
                                            .Where(x => string.IsNullOrWhiteSpace(request.Filter.City) || x.Locality.StartsWith(request.Filter.City))
                                            .Where(x => x.OccursOn > DateTime.Today)
                                            .Include(x=>x.EventType)
                                             .Select(x => new EventResponseModel
                                             {

                                                 Id = x.Id,
                                                 Address = x.Address,
                                                 Author = x.Author,
                                                 ContentBody = x.ContentBody,
                                                 OccursOn = x.OccursOn,
                                                 OrganizedBy = x.OrganizedBy,
                                                 PublishedOn = x.PublishedOn,
                                                 Title = x.Title,
                                                 City = x.Locality,
                                                 EventType = x.EventType.Name,
                                                 IsArchived = x.IsArchived
                                             })
                                             .ToListAsync(ct);

            return events.ToImmutableList();
        }
    }
}