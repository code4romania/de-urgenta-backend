﻿using System;

namespace DeUrgenta.Courses.Api.Models
{
    public sealed record CourseCityModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}