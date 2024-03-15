using FluentEmail.Core.Models;

namespace Hotboil.Mailer;

public class MailerOptions
{
    public static string SectionName => "Mailer";
    public static string SmtpSectionName => "Mailer:Smtp";
    
    public string? FromName { get; set; }
    public string? FromEmail { get; set; }
}