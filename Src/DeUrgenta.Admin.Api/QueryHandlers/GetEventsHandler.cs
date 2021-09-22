using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.QueryHandlers
{
    public class GetEventsHandler : IRequestHandler<GetEvents, Result<PagedResult<EventResponseModel>>>
    {
        private readonly IValidateRequest<GetEvents> _validator;
        private readonly DeUrgentaContext _context;

        public GetEventsHandler(IValidateRequest<GetEvents> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<PagedResult<EventResponseModel>>> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<PagedResult<EventResponseModel>>("Validation failed");
            }

            var pagedEvents = await _context.Events
                .Include(x=>x.EventType)
                .Select(x => new EventResponseModel
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
                    EventType = x.EventType.Name,
                    IsArchived = x.IsArchived
                })
                .OrderBy(x => x.OccursOn)
                .GetPaged(request.Pagination.PageNumber, request.Pagination.PageSize);

            return pagedEvents;
        }
    }
}
