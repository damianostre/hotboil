using System.Reflection;

namespace Hotboil.Mailer;

public abstract class EmailTemplateInfo
{
    public bool IsHtml { get; set; } = true;
}

public class FileEmailTemplateInfo : EmailTemplateInfo
{
    public required string Path { get; set; }    
}

public class EmbeddedEmailTemplateInfo : EmailTemplateInfo
{
    public required string Name { get; set; }
    public required Assembly Assembly { get; set; }
}