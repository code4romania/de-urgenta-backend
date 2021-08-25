using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class DeleteEvent : IRequest<Result>
    {
        public Guid EventId { get; set; }

        public DeleteEvent(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
