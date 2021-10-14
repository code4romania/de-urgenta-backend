using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class EventBuilder
    {
        private int _eventTypeId = new Random().Next();
        private DateTime _date = DateTime.Now;
        
        public Event Build() => new Event
        {
            Title = TestDataProviders.RandomString(),
            OrganizedBy = TestDataProviders.RandomString(),
            ContentBody = TestDataProviders.RandomString(),
            Author = TestDataProviders.RandomString(),
            Address = TestDataProviders.RandomString(),
            City = TestDataProviders.RandomString(),
            EventTypeId = _eventTypeId,
            OccursOn = _date
        };
        
        public EventBuilder WithEventTypeId(int eventTypeId)
        {
            _eventTypeId = eventTypeId;
            return this;

        }

        public EventBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }
    }
}