namespace Hotboil.Mailer.Transports.Sandbox;

public class SandboxMailTransport(string directory) : IMailTransport
{
    public SendResponse Send(EmailData email, CancellationToken? token = null)
    {
        return SendAsync(email, token).GetAwaiter().GetResult();
    }

    public async Task<SendResponse> SendAsync(EmailData email, CancellationToken? token = null)
    {
        var response = new SendResponse();
        await SaveEmailToDisk(email);
        return response;
    }

    private async Task<bool> SaveEmailToDisk(EmailData email)
    {
        var random = new Random();
        var filename = Path.Combine(directory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{random.Next(1000)}");

        using (var sw = new StreamWriter(File.OpenWrite(filename)))
        {
            await sw.WriteLineAsync($"From: {email.FromAddress.Name} <{email.FromAddress.EmailAddress}>");
            await sw.WriteLineAsync($"To: {string.Join(",", email.ToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
            await sw.WriteLineAsync($"Cc: {string.Join(",", email.CcAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
            await sw.WriteLineAsync($"Bcc: {string.Join(",", email.BccAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
            await sw.WriteLineAsync($"ReplyTo: {string.Join(",", email.ReplyToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
            await sw.WriteLineAsync($"Subject: {email.Subject}");
            foreach (var dataHeader in email.Headers)
            {
                await sw.WriteLineAsync($"{dataHeader.Key}:{dataHeader.Value}");
            }
            await sw.WriteLineAsync();
            await sw.WriteAsync(email.Body);
        }

        return true;
    }
}