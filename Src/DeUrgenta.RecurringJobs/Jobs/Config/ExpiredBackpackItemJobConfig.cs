namespace DeUrgenta.RecurringJobs.Jobs.Config
{
    public record ExpiredBackpackItemJobConfig : RecurringJobConfig
    {
        public uint DaysBeforeExpirationDate { get; set; }
    }
}