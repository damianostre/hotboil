using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer;

public class MailerService(IMailTransport sender, ITemplateEngine engine, IOptions<MailerOptions>? options)
    : IMailerService
{
    private readonly MailerOptions _options = options?.Value ?? new MailerOptions();

    public async Task<SendResponse> SendAsync<T>(T mail, CancellationToken token = default) where T : Mail<T>, new()
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
        
        var html = await GetContent(mail, mail.GetHtmlContent());
        var text = await GetContent(mail, mail.GetTextContent());

        data.Body = html ?? text;
        data.PlaintextAlternativeBody = html is not null ? text : null;
        data.IsHtml = html is not null;

        return await sender.SendAsync(data, token);
    }
    
    private async Task<string?> GetContent<T>(T mail, MailContent? mailContent) where T : Mail<T>, new()
    {
        if (mailContent is null) return null;
        
        if (mailContent is FileTemplateMailContent fileTemplate)
        {
            if (File.Exists(fileTemplate.Path) is false)
            {
                throw new InvalidOperationException("Template not found");
            }
            
            using var reader = new StreamReader(File.OpenRead(fileTemplate.Path));
            var template = await reader.ReadToEndAsync();

            return await engine.ParseAsync(template, mail);
        }

        if (mailContent is EmbeddedTemplateMailContent embeddedTemplate)
        {
            var template = EmbeddedResourceHelper.GetResourceAsString(
                embeddedTemplate.Assembly, embeddedTemplate.Name);
            if (template is null)
            {
                throw new InvalidOperationException("Template not found");
            }
            
            return await engine.ParseAsync(template, mail);
        }

        throw new InvalidOperationException("Unknown template type");
    }
} 

public interface IMailerService
{
}