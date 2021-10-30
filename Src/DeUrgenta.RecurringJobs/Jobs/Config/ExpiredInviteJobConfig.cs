namespace DeUrgenta.RecurringJobs.Jobs.Config
{
    public record ExpiredInviteJobConfig : RecurringJobConfig
    {
        public uint DaysBeforeExpirationDate { get; set; }
    }
}
