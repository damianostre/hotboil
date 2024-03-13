using FluentEmail.Core;
using FluentEmail.Core.Interfaces;

namespace Hotboil.Mailer;

public class MailerService : IMailerService
{
    private readonly ISender _sender;
    private readonly ITemplateRenderer? _renderer;

    public async Task SendAsync<T>(T mail) where T : Mail<T>, new()
    {
        var fluentEmail = new Email()
        {
            Data = mail.GetEmailData()
        };

        fluentEmail.Subject(mail.GetSubject());
        var template = mail.GetTemplate();
        if (template is FileEmailTemplateInfo fileTemplate)
        {
            fluentEmail.UsingTemplateEngine()
        }
        else
        {
            fluentEmail.UsingTemplate(template.Template, mail.GetTemplate().Model, _renderer);
        }
    }

    public MailerService(ISender sender, ITemplateRenderer? renderer)
    {
        _sender = sender;
        _renderer = renderer;
    }
} 

public interface IMailerService
{
    
}