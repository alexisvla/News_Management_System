using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Security;


namespace NEWS_WebAplication.Services
{
    public interface IMailSender
    {
        public Task send(string email, string subject, string body);

    }

    public class Mailsender: IMailSender
    {
        private readonly IConfiguration _configuration;

        public Mailsender(IConfiguration configuration)
        {
           _configuration = configuration;
        }

       
        public async Task send(string ToEmail, string subject, string body)
        {
            string Server = _configuration["MailSettings:Server"];
            string Port = _configuration["MailSettings:Port"];
            string FromEmail = _configuration["MailSettings:UserName"];
            string Username = _configuration["MailSettings:UserName"];
            string password = _configuration["MailSettings:Password"];
           

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("",FromEmail));
            message.To.Add(new MailboxAddress("",ToEmail));
            message.Subject = subject; 
           
            message.Body = new BodyBuilder() { HtmlBody = body }.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(Server, int.Parse(Port), false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(Username, password);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        

    }
}
