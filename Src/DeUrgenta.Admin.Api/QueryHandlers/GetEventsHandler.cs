using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.QueryHandlers
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
                .Select(x => new EventModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Address = x.Address,
                    Author = x.Author,
                    ContentBody = x.ContentBody,
                    OccursOn = x.OccursOn,
                    OrganizedBy = x.OrganizedBy,
                    PublishedOn = x.PublishedOn,
                    City = x.City,
                    EventTypeId = x.EventTypeId
                })
                .OrderBy(x => x.OccursOn)
                .ToListAsync(cancellationToken);

            return events.ToImmutableList();
        }
    }
}
