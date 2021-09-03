using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class UpdateEventHandler : IRequestHandler<UpdateEvent, Result<EventModel>>
    {
        private readonly IValidateRequest<UpdateEvent> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateEventHandler(IValidateRequest<UpdateEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<EventModel>> Handle(UpdateEvent request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<EventModel>("Validation failed");
            }
            var ev = await _context.Events.FirstAsync(b => b.Id == request.EventId, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new EventModel
            {
                Id = ev.Id,
                Title = ev.Title,
                Address = ev.Address,
                Author = ev.Author,
                City = ev.City,
                ContentBody = ev.ContentBody,
                EventTypeId = ev.EventTypeId,
                OccursOn = ev.OccursOn,
                OrganizedBy = ev.OrganizedBy
            };
        }
    }
}
