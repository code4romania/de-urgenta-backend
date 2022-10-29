﻿using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IAmContentProvider //Todo add ct support on methods
    {
        Task<StringResourceModel> AddOrUpdateContentValue(string culture, string resourceKey, string resourceValue);

        Task<ImmutableList<string>> GetAvailableContentKeys(string culture);
    }
}