using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class DeleteEvent : IRequest<Result<Unit, ValidationResult>>
    {
        public Guid EventId { get; set; }

        public DeleteEvent(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
