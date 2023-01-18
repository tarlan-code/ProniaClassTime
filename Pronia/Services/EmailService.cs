using Pronia.Abstractions.Services;
using System.Net;
using System.Net.Mail;

namespace Pronia.Services
{
    public class EmailService:IEmailService
    {
        IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void Send(string mailTo, string subject, string body,bool IsHtml = false)
        {
            SmtpClient smtpClient = new SmtpClient(_config["Email:Host"], Convert.ToInt32(_config["Email:Port"]));
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_config["Email:Mail"], _config["Email:Password"]);


            MailAddress from = new MailAddress(_config["Email:Mail"], "Pronia");
            MailAddress to = new MailAddress(mailTo);  //"heyderovterlan8@gmail.com"

            MailMessage msg = new MailMessage(from,to);
            msg.Subject = subject; //"asp.net Pronia Project";
            msg.Body = body;  //"Bu email Pronia mvc layihesi uchun bir emaildir";

           
            smtpClient.Send(msg);
            

        }
    }
}
