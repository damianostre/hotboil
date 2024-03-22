// https://github.com/lukencode/FluentEmail/blob/master/src/Senders/FluentEmail.MailKit/SmtpClientOptions.cs

using MailKit.Security;

namespace Hotboil.Mailer.Transports.Smtp;

public class SmtpClientOptions
{
    public static string SectionName => "Mailer:Smtp";
    
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; } = 25;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = false;
    public bool RequiresAuthentication { get; set; } = false;
    public string PreferredEncoding { get; set; } = string.Empty;
    public bool UsePickupDirectory { get; set; } = false;
    public string MailPickupDirectory { get; set; } = string.Empty;
    public SecureSocketOptions? SocketOptions { get; set; }
}