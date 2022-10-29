using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class UpdateEventHandler : IRequestHandler<UpdateEvent, Result<EventResponseModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateEvent> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateEventHandler(IValidateRequest<UpdateEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<EventResponseModel, ValidationResult>> Handle(UpdateEvent request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var eventType = await _context.EventTypes.FirstAsync(et => et.Id == request.Event.EventTypeId, ct);
            var @event = await _context.Events.FirstAsync(e => e.Id == request.EventId, ct);

            @event.Address = request.Event.Address;
            @event.Author = request.Event.Author;
            @event.Locality = request.Event.Locality;
            @event.ContentBody = request.Event.ContentBody;
            @event.EventType = eventType;
            @event.OccursOn = request.Event.OccursOn;
            @event.OrganizedBy = request.Event.OrganizedBy;
            @event.Title = request.Event.Title;
            @event.IsArchived = request.Event.IsArchived;
            @event.PublishedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);

            return new EventResponseModel
            {
                Id = @event.Id,
                Title = @event.Title,
                Address = @event.Address,
                Author = @event.Author,
                City = @event.Locality,
                ContentBody = @event.ContentBody,
                EventType = eventType.Name,
                OccursOn = @event.OccursOn,
                OrganizedBy = @event.OrganizedBy,
                IsArchived = @event.IsArchived,
                PublishedOn = @event.PublishedOn
            };
        }
    }
}
