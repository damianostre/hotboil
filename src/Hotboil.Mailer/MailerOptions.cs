namespace Hotboil.Mailer;

public class MailerOptions
{
    public static string SectionName => "Mailer";
    
    public string? FromName { get; set; }
    public string? FromEmail { get; set; }
    
    public MailerSandboxOptions? Sandbox { get; set; }
}

public class MailerSandboxOptions
{
    public static string SectionName => "Mailer:Sandbox";
    
    public MailerSandboxMode? Mode { get; set; }
    public string? Directory { get; set; }
    public List<string> AllowedDomains { get; set; } = new();
    public List<string> AllowedEmails { get; set; } = new();
    public string? ForwardTo { get; set; }
}

public enum MailerSandboxMode
{
    Disabled = 0,
    SaveToFile = 1,
    InMemory = 2,
    Forward = 3,
    AllowList = 4,
}