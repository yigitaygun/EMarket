using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Services;

namespace EMarketAPI.Infrastructure.Services
{
    public class MailtrapMailService : IMailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("f7b6be13224c13", "1ddb314b497f38"),
                EnableSsl = true
            };

            var mail = new MailMessage("noreply@emarketapi.com", to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }

    }
}
