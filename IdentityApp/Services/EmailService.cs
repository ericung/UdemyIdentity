﻿using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using IdentityApp.Settings;

namespace IdentityApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSetting> smtpSetting;

        public EmailService(IOptions<SmtpSetting> smtpSetting)
        {
            this.smtpSetting = smtpSetting;
        }

        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from,
                    to,
                    subject,
                    body);

            using (var emailClient = new SmtpClient(
                this.smtpSetting.Value.Host,
                this.smtpSetting.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    this.smtpSetting.Value.User,
                    this.smtpSetting.Value.Password);

                await emailClient.SendMailAsync(message);
            }
        }
    }
}
