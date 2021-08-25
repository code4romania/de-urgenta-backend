using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Models;
using System.Collections.Generic;

namespace DeUrgenta.Courses.Api.QueryHandlers
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

            List<EventModel> events;
            if (request.ModelRequest.CityId == null && request.ModelRequest.EventTypeId == null)
            {
                events = await _context.Events
                                          .Select(x => new EventModel
                                          {
                                              Id = x.Id,
                                              Author = x.Author,
                                              ContentBody = x.ContentBody,
                                              OccursOn = x.OccursOn,
                                              OrganizedBy = x.OrganizedBy,
                                              PublishedOn = x.PublishedOn,
                                              Title = x.Title,
                                              City = x.City.Name,
                                              EventType = x.EventType.Name
                                          })
                                          .ToListAsync(cancellationToken);
            }
            else if (request.ModelRequest.EventTypeId == null)
            {
                events = await _context.Events
                                         .Where(x => request.ModelRequest.CityId == x.CityId)
                                         .Select(x => new EventModel
                                         {
                                             Id = x.Id,
                                             Author = x.Author,
                                             ContentBody = x.ContentBody,
                                             OccursOn = x.OccursOn,
                                             OrganizedBy = x.OrganizedBy,
                                             PublishedOn = x.PublishedOn,
                                             Title = x.Title,
                                             City = x.City.Name,
                                             EventType = x.EventType.Name
                                         })
                                         .ToListAsync(cancellationToken);
            }
            else if (request.ModelRequest.CityId == null)
            {
                events = await _context.Events
                                         .Where(x => request.ModelRequest.EventTypeId == x.TypeId)
                                         .Select(x => new EventModel
                                         {
                                             Id = x.Id,
                                             Author = x.Author,
                                             ContentBody = x.ContentBody,
                                             OccursOn = x.OccursOn,
                                             OrganizedBy = x.OrganizedBy,
                                             PublishedOn = x.PublishedOn,
                                             Title = x.Title,
                                             City = x.City.Name,
                                             EventType = x.EventType.Name
                                         })
                                         .ToListAsync(cancellationToken);
            }
            else
            {
                events = await _context.Events
                                         .Where(x => request.ModelRequest.CityId == x.CityId && request.ModelRequest.EventTypeId == x.TypeId)
                                         .Select(x => new EventModel
                                         {
                                             Id = x.Id,
                                             Author = x.Author,
                                             ContentBody = x.ContentBody,
                                             OccursOn = x.OccursOn,
                                             OrganizedBy = x.OrganizedBy,
                                             PublishedOn = x.PublishedOn,
                                             Title = x.Title,
                                             City = x.City.Name,
                                             EventType = x.EventType.Name
                                         })
                                         .ToListAsync(cancellationToken);
            }
            return events.ToImmutableList();
        }
    }
}