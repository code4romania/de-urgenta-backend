using System;
using System.Collections.Immutable;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Models.Events;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.Events
{
    public class GetEventsResponseExample : IExamplesProvider<PagedResult<EventResponseModel>>
    {
        public PagedResult<EventResponseModel> GetExamples()
        {
            return new()
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 25,
                RowCount = 3,
                Results = new EventResponseModel[]
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Author = "Albert Einstein",
                        ContentBody = "<h1> E = mc^2 ajutor.</h1>",
                        Title = "Curs relativ prim ajutor",
                        OccursOn = DateTime.Today.AddDays(30),
                        OrganizedBy = "Crucea rosie",
                        PublishedOn = DateTime.Today.AddDays(-240),
                        EventType = "Prim ajutor",
                        IsArchived = false,
                        Address = "Strada Luminii",
                        City = "Iasi"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Author = "Freddie mercury",
                        ContentBody = "<h1> Mama, I just saved a man.... uuuuuuu</h1>",
                        Title = "First Aid Live",
                        OccursOn = DateTime.Today.AddDays(62),
                        OrganizedBy = "Queen First Aid Foundation",
                        PublishedOn = DateTime.Today.AddDays(-39),
                        EventType = "Prim ajutor calificat",
                        IsArchived = false,
                        Address = "Bulevardul Victoria",
                        City = "Bucuresti"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Author = "Disco Dancer",
                        Title = "Save lives in style.",
                        OccursOn = DateTime.Today.AddDays(62),
                        OrganizedBy = "Staying Alive Foundation",
                        PublishedOn = DateTime.Today,
                        ContentBody = @"
                        <h1>Save lives in style</h1>
                    <p>Well, you can tell by the way I use my walk</p>
                    <p>I'm a woman's man, no time to talk</p>
                    <p>Music loud and women warm,&nbsp;</p>
                    <p>I've been kicked around since I was born</p>
                    <p>
                        <br>
                    </p>
                    <p>And now it's all right, it's okay</p>
                    <p>And you may look the other way</p>
                    <p>We can try to understand&nbsp;</p>
                    <p>The New York Times' effect on man</p>
                    <p>
                        <br>
                    </p>
                    <p>Whether you're a brother or whether you're a mother</p>
                    <p>You're stayin' alive, stayin' alive</p>
                    <p>Feel the city breakin' and everybody shakin'</p>
                    <p>And we're stayin' alive, stayin' alive</p>
                    <p>Ah, ha, ha, ha, stayin' alive, stayin' alive</p>
                    <p>Ah, ha, ha, ha, stayin' alive</p>
                    <p>
                        <br>
                    </p>
                    <p>Well now, I get low and I get high</p>
                    <p>And if I can't get either, I really try</p>
                    <p>Got the wings of heaven on my shoes</p>
                    <p>I'm a dancing man and I just can't lose</p>
                    <p>You know it's all right, it's okay</p>
                    <p>I'll live to see another day</p>
                    <p>We can try to understand&nbsp;</p>
                    <p>The New York Times' effect on man</p>
                    <p>
                        <br>
                    </p>
                    <p>Whether you're a brother or whether you're a mother</p>
                    <p>You're stayin' alive, stayin' alive</p>
                    <p>Feel the city breakin' and everybody shakin'</p>
                    <p>And were stayin' alive, stayin' alive</p>
                    <p>Ah, ha, ha, ha, stayin' alive, stayin' alive</p>
                    <p>Ah, ha, ha, ha, stayin' alive</p>
                    <p>
                        <br>
                    </p>
                    <p>Life going nowhere, somebody help me</p>
                    <p>Somebody help me, yeah</p>
                    <p>Life going nowhere, somebody help me</p>
                    <p>Somebody help me, yeah&nbsp;</p>
                    <ul>
                        <li>Stayin' alive</li>
                    </ul>",
                        EventType = "Prim ajutor",
                        IsArchived = false,
                        Address = "Pregatire in caz de dezastre",
                        City = "Bacau"
                    },
                }.ToImmutableList()
            };
        }
    }
}