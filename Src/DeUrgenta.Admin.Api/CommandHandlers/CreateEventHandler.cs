using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class CreateEventHandler : IRequestHandler<CreateEvent, Result<EventModel>>
    {
        private readonly IValidateRequest<CreateEvent> _validator;
        private readonly DeUrgentaContext _context;

        public CreateEventHandler(IValidateRequest<CreateEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<EventModel>> Handle(CreateEvent request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<EventModel>("Validation failed");
            }

            var ev = new Domain.Entities.Event 
            { 
                Title = request.Event.Title,
                Author = request.Event.Author,
                City = request.Event.City,
                ContentBody = request.Event.ContentBody,
                EventTypeId = request.Event.EventTypeId,
                OccursOn = request.Event.OccursOn,
                OrganizedBy = request.Event.OrganizedBy,
                Address = request.Event.Address,
            };

            await _context.Events.AddAsync(ev, cancellationToken);
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
