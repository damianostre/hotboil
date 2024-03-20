using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer;

public class MailerService(IMailTransport sender, ITemplateEngine engine, IOptions<MailerOptions>? options)
    : IMailerService
{
    private readonly MailerOptions _options = options?.Value ?? new MailerOptions();

    public async Task SendAsync<T>(T mail, CancellationToken token = default) where T : Mail<T>, new()
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
        
        var htmlContent = mail.GetHtmlContent();
        var textContent = mail.GetTextContent();
        
        if (htmlContent is FileTemplateMailContent fileTemplate)
        {
            data.Body = await engine.ParseAsync(fileTemplate.Path, mail);
            data.IsHtml = true;
        }
        else if (htmlContent is EmbeddedTemplateMailContent embeddedTemplate)
        {
            var template = EmbeddedResourceHelper.GetResourceAsString(
                embeddedTemplate.Assembly, embeddedTemplate.Name);
            var result = await engine.ParseAsync(template, mail);
            data.IsHtml = true;
            data.Body = result;
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