using System.Collections.Generic;
using DeUrgenta.Emailing.Service.Models;

namespace DeUrgenta.Emailing.Service.Constants
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
            { EmailTemplate.AccountConfirmation, "Bine ai venit!" },
            { EmailTemplate.ResetPassword, "Resetare parolă" },
            { EmailTemplate.ExpiredCertification, "Certificare expirată" },
            { EmailTemplate.ExpiredBackBackpackItem, "Produs din rucsac expirat" }
        };

        private static readonly Dictionary<EmailTemplate, string> TemplatePaths = new()
        {
            { EmailTemplate.AccountConfirmation, "accountConfirmationTemplate.html" },
            { EmailTemplate.ResetPassword, "resetPasswordTemplate.html" },
            { EmailTemplate.ExpiredCertification, "expiredCertification.html" },
            { EmailTemplate.ExpiredBackBackpackItem, "expiredBackpackItem.html" }
        };
    }
}
