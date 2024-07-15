using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

public class SendMailService : IEmailSender
{
    private readonly MailSetting mailSetting;
    private readonly ILogger<SendMailService> logger;
    public SendMailService(IOptions<MailSetting> _mailSetting, ILogger<SendMailService> _logger)
    {
        mailSetting = _mailSetting.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(mailSetting.DisplayName, mailSetting.Mail);
        message.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Mail));
        message.Subject = subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = body;
        message.Body = builder.ToMessageBody();

        using var stmp = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            stmp.Connect(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
            stmp.Authenticate(mailSetting.Mail, mailSetting.Password);
            await stmp.SendAsync(message);
        }
        catch (Exception ex)
        {
            System.IO.Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);

            logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
            logger.LogError(ex.Message);
        }
        stmp.Disconnect(true);

        logger.LogInformation("send mail to: " + email);
    }

}

public class MailSetting
{
    public string Mail { set; get; }
    public string DisplayName { set; get; }
    public string Password { set; get; }
    public string Host { set; get; }
    public int Port { set; get; }
}

