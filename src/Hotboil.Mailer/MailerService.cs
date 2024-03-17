using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer;

public class MailerService(IMailTransport sender, ITemplateEngine renderer, IOptions<MailerOptions>? options)
    : IMailerService
{
    private readonly MailerOptions _options = options?.Value ?? new MailerOptions();

    public Task SendAsync<T>(T mail, CancellationToken token = default) where T : Mail<T>, new()
    {
        // var fluentEmail = new Email(renderer, sender)
        // {
        //     Data = mail.GetEmailData()
        // };
        var data = mail.GetEmailData();
        data.Subject = mail.GetSubject();
        if (fluentEmail.Data.FromAddress is null)
        {
            var from = mail.GetFrom() ?? new Address(_options.FromEmail, _options.FromName);
            fluentEmail.SetFrom(from.EmailAddress, from.Name);
        }
        
        var template = mail.GetTemplate();
        if (template is FileEmailTemplateInfo fileTemplate)
        {
            fluentEmail.UsingTemplateFromFile(fileTemplate.Path, mail, fileTemplate.IsHtml);
        }
        else if (template is EmbeddedEmailTemplateInfo embeddedTemplate)
        {
            fluentEmail.UsingTemplateFromEmbedded(
                embeddedTemplate.Name, mail, embeddedTemplate.Assembly, embeddedTemplate.IsHtml);
        }
        else
        {
            throw new InvalidOperationException("Unknown template type");
        }
        
        return sender.SendAsync(fluentEmail, token);
    }
} 

public interface IMailerService
{
}