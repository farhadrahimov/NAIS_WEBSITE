using NAIS_Website.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

namespace NAIS_Website.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailModel model);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(EmailModel model)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(model.Name, model.Email));
                //email.ReplyTo.Add(new MailboxAddress(model.Name, model.Email));
                email.To.Add(new MailboxAddress("", "farxadjr@gmail.com"));
                email.Subject = "Сообщение от " + model.Name;

                email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = $"Имя: {model.Name}\nEmail: {model.Email}\nСообщение:\n{model.Message}"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync("farxadjr@gmail.com", "ijxg hmyx kfdq bkhr");
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
