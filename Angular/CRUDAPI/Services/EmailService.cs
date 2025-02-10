using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer = "live.smtp.mailtrap.io"; // Exemplo: smtp.gmail.com
    private readonly int _smtpPort = 587;
    private readonly string _emailFrom = "hello@demomailtrap.com"; // Melhor armazenar isso em variáveis de ambiente
    private readonly string _emailPassword = "155e34e621d4d4a1b25bb7058e911616"; // Melhor armazenar isso em variáveis de ambiente

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_emailFrom, _emailPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailFrom),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
