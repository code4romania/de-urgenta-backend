namespace DeUrgenta.RecurringJobs.Jobs.Config
{
    public record NotificationSenderJobConfig : RecurringJobConfig
    {
        public int BatchSize { get; set; }
    }
}
