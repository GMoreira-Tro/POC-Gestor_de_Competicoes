using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com"; // Exemplo: smtp.gmail.com
    private readonly int _smtpPort = 587;
    private readonly string _emailFrom = "guilherme.dsmoreira@gmail.com"; // Melhor armazenar isso em variáveis de ambiente
    private readonly string _emailPassword = "blru ewmu zila huwd"; // Melhor armazenar isso em variáveis de ambiente

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_emailFrom, _emailPassword),
            EnableSsl = true,
            UseDefaultCredentials = false
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
