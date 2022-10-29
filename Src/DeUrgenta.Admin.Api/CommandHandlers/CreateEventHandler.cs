using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class CreateEventHandler : IRequestHandler<CreateEvent, Result<EventResponseModel, ValidationResult>>
    {
        private readonly IValidateRequest<CreateEvent> _validator;
        private readonly DeUrgentaContext _context;

        public CreateEventHandler(IValidateRequest<CreateEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<EventResponseModel, ValidationResult>> Handle(CreateEvent request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }
            var eventType = await _context.EventTypes.FirstAsync(et => et.Id == request.Event.EventTypeId, ct);

            var @event = new Event
            {
                Title = request.Event.Title,
                Author = request.Event.Author,
                Locality = request.Event.Locality,
                ContentBody = request.Event.ContentBody,
                EventType = eventType,
                OccursOn = request.Event.OccursOn,
                OrganizedBy = request.Event.OrganizedBy,
                Address = request.Event.Address,
                IsArchived = request.Event.IsArchived,
                PublishedOn = DateTime.UtcNow
            };

            await _context.Events.AddAsync(@event, ct);
            await _context.SaveChangesAsync(ct);

            return new EventResponseModel
            {
                Id = @event.Id,
                Title = @event.Title,
                Address = @event.Address,
                Author = @event.Author,
                City = @event.Locality,
                IsArchived = @event.IsArchived,
                ContentBody = @event.ContentBody,
                EventType = eventType.Name,
                OccursOn = @event.OccursOn,
                OrganizedBy = @event.OrganizedBy,
                PublishedOn = @event.PublishedOn
            };
        }
    }
}
