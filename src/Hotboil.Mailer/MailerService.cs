using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer;

public class MailerService(IMailTransport sender, ITemplateEngine renderer, IOptions<MailerOptions>? options)
    : IMailerService
{
    private readonly MailerOptions _options = options?.Value ?? new MailerOptions();

    public Task SendAsync<T>(T mail, CancellationToken token = default) where T : Mail<T>, new()
    {
        var data = mail.GetEmailData();
        data.Subject = mail.GetSubject();
        data.FromAddress ??= mail.GetFromAddress() ?? (_options.FromEmail is not null
            ? new Address(_options.FromEmail, _options.FromName)
            : null);
        
        if (data.FromAddress is null)
        {
            throw new InvalidOperationException("From address is not set");
        }
        
        // var template = mail.GetHtmlContent();
        // if (template is FileTemplateMailContent fileTemplate)
        // {
        //     new Email().PlaintextAlternativeUsingTemplate(fileTemplate.Path, mail, fileTemplate.IsHtml);
        // }
        // else if (template is EmbeddedFileMailContent embeddedTemplate)
        // {
        //     fluentEmail.UsingTemplateFromEmbedded(
        //         embeddedTemplate.Name, mail, embeddedTemplate.Assembly, embeddedTemplate.IsHtml);
        // }
        // else
        // {
        //     throw new InvalidOperationException("Unknown template type");
        // }
        
        return sender.SendAsync(fluentEmail, token);
    }
} 

public interface IMailerService
{
}