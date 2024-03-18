using System.Reflection;

namespace Hotboil.Mailer;

public abstract class MailContent
{
}

public class FileTemplateMailContent : MailContent
{
    public required string Path { get; set; }
}

public class EmbeddedTemplateMailContent : MailContent
{
    public required string Name { get; set; }
    public required Assembly Assembly { get; set; }
}