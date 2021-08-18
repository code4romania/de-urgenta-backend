using System.Collections.Generic;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public static class EmailConstants
    {
        public static string GetSubject(EmailTemplate template)
        {
            return subjects[template];
        }

        public static string GetTemplatePath(EmailTemplate template)
        {
            return templatePaths[template];
        }

        private static Dictionary<EmailTemplate, string> subjects = new Dictionary<EmailTemplate, string>
        {
            { EmailTemplate.AccountConfirmation, "Bine ai venit!"},
            { EmailTemplate.ResetPassword, "Resetare parolă"}
        };

        private static Dictionary<EmailTemplate, string> templatePaths = new Dictionary<EmailTemplate, string>
        {
            { EmailTemplate.AccountConfirmation, "accountConfirmationTemplate.html"},
            { EmailTemplate.ResetPassword, "resetPasswordTemplate.html"}
        };
    }
}
