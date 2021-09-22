using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Models.Events;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class UpdateEvent : IRequest<Result<EventResponseModel>>
    {
        public Guid EventId { get; }
        public EventRequest Event { get; }

        public UpdateEvent(Guid eventId, EventRequest eventModel)
        {
            EventId = eventId;
            Event = eventModel;
        }
    }
}
