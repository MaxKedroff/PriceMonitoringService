using MimeKit;
using MailKit.Net.Smtp;

using System.Net.Mail;
using MailKit.Security;

namespace PriceMonitoringService.Service
{
    public class EmailService
    {
       public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "MaxKed12345@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("MaxKed12345@yandex.ru", "doqjojsfuabagxnr");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
