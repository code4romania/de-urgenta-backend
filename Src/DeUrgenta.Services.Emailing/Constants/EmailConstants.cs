using System.Collections.Generic;
using DeUrgenta.Services.Emailing.Models;

namespace DeUrgenta.Services.Emailing.Constants
{
    public static class EmailConstants
    {
        public static string GetSubject(EmailTemplate template)
        {
            return Subjects[template];
        }

        public static string GetTemplatePath(EmailTemplate template)
        {
            return TemplatePaths[template];
        }

        private static readonly Dictionary<EmailTemplate, string> Subjects = new()
        {
            { EmailTemplate.AccountConfirmation, "Bine ai venit!"},
            { EmailTemplate.ResetPassword, "Resetare parolă"}
        };

        private static readonly Dictionary<EmailTemplate, string> TemplatePaths = new()
        {
            { EmailTemplate.AccountConfirmation, "accountConfirmationTemplate.html"},
            { EmailTemplate.ResetPassword, "resetPasswordTemplate.html"}
        };
    }
}
