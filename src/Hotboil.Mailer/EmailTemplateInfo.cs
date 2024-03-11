namespace Hotboil.Mailer;

public abstract class EmailTemplateInfo
{
    public bool IsHtml { get; set; }
}

public class FileEmailTemplateInfo : EmailTemplateInfo
{
    public string Path { get; set; }    
}

public class EmbeddedEmailTemplateInfo : EmailTemplateInfo
{
    public string Name { get; set; }
}