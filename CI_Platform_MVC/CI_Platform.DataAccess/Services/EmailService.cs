using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(SmtpClient smtpClient, string fromEmail)
        {
            _smtpClient = smtpClient;
            _fromEmail = fromEmail;
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage(_fromEmail, email, subject, message)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
