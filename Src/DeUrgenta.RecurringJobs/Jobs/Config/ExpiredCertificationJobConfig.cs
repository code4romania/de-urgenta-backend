namespace DeUrgenta.RecurringJobs.Jobs.Config
{
    public record ExpiredCertificationJobConfig : RecurringJobConfig
    {
        public uint DaysBeforeExpirationDate { get; set; }
    }
}