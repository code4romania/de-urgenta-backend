﻿using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface IExpiredCertificationJob
    {
        Task RunAsync();
    }
}