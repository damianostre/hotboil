namespace Hotboil.Mailer;

public class MailerOptions
{
    public static string SectionName => "Mailer";
    
    public string? FromName { get; set; }
    public string? FromEmail { get; set; }
    
    public List<string> AllowedDomains { get; set; } = new();
    public List<string> AllowedEmails { get; set; } = new();
    public string? ForwardTo { get; set; }
    
    public MailerSandboxOptions? Sandbox { get; set; }
}

public class MailerSandboxOptions
{
    public static string SectionName => "Mailer:Sandbox";
    
    public bool Enabled { get; set; }
    public string? SaveToDir { get; set; }
    public bool InMemory { get; set; }
    
}
