namespace CaseApplication.Api.Services
{
    public class EmailHelper
    {
        private readonly EmailService _emailService;

        public EmailHelper(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> SendDeleteAccountToEmail()
        {

        }
    }
}
