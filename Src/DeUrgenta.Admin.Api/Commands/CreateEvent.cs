using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class CreateEvent : IRequest<Result<EventResponseModel, ValidationResult>>
    {
        public EventRequest Event { get; }

        public CreateEvent(EventRequest eventRequest)
        {
            Event = eventRequest;
        }
    }
}
