namespace Hotboil.Mailer;

public class MailerOptions
{
    public static string SectionName => "Mailer";
    
    public string? FromName { get; set; }
    public string? FromEmail { get; set; }
}