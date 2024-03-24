using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hotboil.Mailer.Transports.Sandbox;

public class SandboxMailTransport(IOptions<MailerOptions> options, ILogger<SandboxMailTransport> logger)
{
    public static ConcurrentBag<EmailData> Mailbox { get; } = new();
    
    private readonly MailerOptions _options = options.Value;

    public async Task<SendResponse?> TryHandle(EmailData email, CancellationToken? token = null)
    {
        if (_options.Sandbox is null || _options.Sandbox.Enabled is false)
        {
            return null;
        }
        
        var directory = _options.Sandbox.SaveToDir;
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
            await WriteEmail(email, directory);
        }
        
        if (_options.Sandbox.InMemory)
        {
            Mailbox.Add(email);
        }

        return new SendResponse();
    }

    private async Task WriteEmail(EmailData email, string directory)
    {
        var random = new Random();
        var filename = Path.Combine(directory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{random.Next(1000)}");

        await using var sw = new StreamWriter(File.OpenWrite(filename));
        await sw.WriteLineAsync($"From: {email.FromAddress?.Name} <{email.FromAddress?.EmailAddress}>");
        await sw.WriteLineAsync(
            $"To: {string.Join(",", email.ToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
        await sw.WriteLineAsync(
            $"Cc: {string.Join(",", email.CcAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
        await sw.WriteLineAsync(
            $"Bcc: {string.Join(",", email.BccAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
        await sw.WriteLineAsync(
            $"ReplyTo: {string.Join(",", email.ReplyToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
        await sw.WriteLineAsync($"Subject: {email.Subject}");
        foreach (var dataHeader in email.Headers)
        {
            await sw.WriteLineAsync($"{dataHeader.Key}:{dataHeader.Value}");
        }

        await sw.WriteLineAsync();
        await sw.WriteAsync(email.Body);
    }
}