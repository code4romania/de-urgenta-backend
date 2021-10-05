namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public record NotificationSendersConfig
    {
        public bool NoOpNotificationSenderEnabled { get; set; }
        public bool EmailNotificationSenderEnabled { get; set; }
        public bool PushNotificationSenderEnabled { get; set; }
    }
}
