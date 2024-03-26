using Hotboil.Mailer.TemplateEngines;
using Hotboil.Mailer.Transports;
using Hotboil.Mailer.Transports.Sandbox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer;

public class MailerService(
    IMailTransport sender, 
    ITemplateEngine engine, 
    IOptions<MailerOptions> options, 
    SandboxMailTransport sandbox,
    ILogger<MailerService> logger) : IMailerService
{
    private readonly MailerOptions _options = options.Value;

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
        
        // Try to send the email using the sandbox transport
        var sandboxed = await sandbox.TryHandle(data, token);
        if (sandboxed is not null) return sandboxed;

        var (firewalled, forwardedFrom) = ApplyFirewallRules(data);
        if (firewalled is not null) return firewalled;
        
        var response = await sender.SendAsync(data, token);
        if (forwardedFrom is not null)
        {
            response.Comment = "Forwarded from " + forwardedFrom;
        }

        return response;
    }

    private (SendResponse? response, string? forwardedFrom) ApplyFirewallRules(EmailData data)
    {
        if (!string.IsNullOrWhiteSpace(_options.ForwardTo))
        {
            var forwardedFrom = string.Join(",", data.ToAddresses.Select(x => x.EmailAddress));
            data.ToAddresses.Clear();
            data.ToAddresses.Add(new Address(_options.ForwardTo));
            
            return (null, forwardedFrom);
        }

        if (_options.AllowedEmails.Count > 0)
        {
            logger.LogInformation("Filtering email addresses");
            data.ToAddresses.RemoveAll(x => _options.AllowedEmails.Contains(x.EmailAddress) is false);
            
            if (data.ToAddresses.Count == 0)
            {
                return (new SendResponse { ErrorMessage = "No email addresses left after filtering" }, null);
            }
        }
        else if (_options.AllowedDomains.Count > 0)
        {
            data.ToAddresses.RemoveAll(x => _options.AllowedDomains.Contains(x.EmailAddress.Split('@')[1]) is false);
            
            if (data.ToAddresses.Count == 0)
            {
                return (new SendResponse { ErrorMessage = "No email addresses left after filtering" }, null);
            }
        }
        
        return (null, null);
    }

    private async Task<string?> GetContent<T>(T mail, MailContent? mailContent) where T : Mail<T>, new()
    {
        if (mailContent is null) return null;
        
        switch (mailContent)
        {
            case StringMailContent stringContent:
                return stringContent.Content;
            case FileTemplateMailContent fileTemplate when File.Exists(fileTemplate.Path) is false:
                throw new InvalidOperationException("Template not found");
            case FileTemplateMailContent fileTemplate:
            {
                using var reader = new StreamReader(File.OpenRead(fileTemplate.Path));
                var template = await reader.ReadToEndAsync();

                return await engine.ParseAsync(template, mail);
            }
            case EmbeddedTemplateMailContent embeddedTemplate:
            {
                var template = EmbeddedResourceHelper.GetResourceAsString(
                    embeddedTemplate.Assembly, embeddedTemplate.Name);
                if (template is null)
                {
                    throw new InvalidOperationException("Template not found");
                }
            
                return await engine.ParseAsync(template, mail);
            }
            default:
                throw new InvalidOperationException("Unknown template type");
        }
    }
} 

public interface IMailerService
{
    Task<SendResponse> SendAsync<T>(T mail, CancellationToken token = default) where T : Mail<T>, new();
}