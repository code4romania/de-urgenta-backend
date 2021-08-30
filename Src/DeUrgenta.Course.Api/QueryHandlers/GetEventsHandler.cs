using CSharpFunctionalExtensions;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Models;

namespace DeUrgenta.Events.Api.QueryHandlers
{
    public class GetEventsHandler : IRequestHandler<GetEvents, Result<IImmutableList<EventModel>>>
    {
        private readonly IValidateRequest<GetEvents> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventsHandler(IValidateRequest<GetEvents> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<EventModel>>> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<EventModel>>("Validation failed");
            }

            var events = await _context.Events
                                            .Where(x => request.ModelRequest.EventTypeId == null || x.EventType.Id == request.ModelRequest.EventTypeId.Value)
                                            .Where(x => string.IsNullOrWhiteSpace(request.ModelRequest.City) || x.City.StartsWith(request.ModelRequest.City))
                                             .Select(x => new EventModel
                                             {
                                                 Id = x.Id,
                                                 Address = x.Address,
                                                 Author = x.Author,
                                                 ContentBody = x.ContentBody,
                                                 OccursOn = x.OccursOn,
                                                 OrganizedBy = x.OrganizedBy,
                                                 PublishedOn = x.PublishedOn,
                                                 Title = x.Title,
                                                 City = x.City,
                                                 EventTypeId = x.EventTypeId
                                             })
                                             .ToListAsync();

            return events.ToImmutableList();
        }
    }
}