namespace DeUrgenta.RecurringJobs.Jobs.Config
{
    public record AcceptedInviteJobConfig : RecurringJobConfig
    {
        public uint DaysBeforeExpirationDate { get; set; }
    }
}
