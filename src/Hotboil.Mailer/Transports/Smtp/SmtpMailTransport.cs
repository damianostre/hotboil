// https://github.com/lukencode/FluentEmail/blob/master/src/Senders/FluentEmail.MailKit/MailKitSender.cs

using System.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Hotboil.Mailer.Transports.Smtp;

/// <summary>
/// Send emails with the MailKit Library.
/// </summary>
public class SmtpMailTransport : IMailTransport
{
    private readonly SmtpClientOptions _smtpClientOptions;

    /// <summary>
    /// Creates a sender that uses the given SmtpClientOptions when sending with MailKit. Since the client is internal this will dispose of the client.
    /// </summary>
    /// <param name="smtpClientOptions">The SmtpClientOptions to use to create the MailKit client</param>
    public SmtpMailTransport(IOptions<SmtpClientOptions> smtpClientOptions)
    {
        _smtpClientOptions = smtpClientOptions.Value;
    }

    /// <summary>
    /// Send the specified email.
    /// </summary>
    /// <returns>A response with any errors and a success boolean.</returns>
    /// <param name="email">Email.</param>
    /// <param name="token">Cancellation Token.</param>
    public SendResponse Send(EmailData email, CancellationToken? token = null)
    {
        return SendAsync(email, token)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Send the specified email.
    /// </summary>
    /// <returns>A response with any errors and a success boolean.</returns>
    /// <param name="email">Email.</param>
    /// <param name="token">Cancellation Token.</param>
    public async Task<SendResponse> SendAsync(EmailData email, CancellationToken? token = null)
    {
        var response = new SendResponse();
        var message = CreateMailMessage(email);

        if (token?.IsCancellationRequested ?? false)
        {
            response.ErrorMessage = "Message was cancelled by cancellation token.";
            return response;
        }

        try
        {
            if (_smtpClientOptions.UsePickupDirectory)
            {
                await this.SaveToPickupDirectory(message, _smtpClientOptions.MailPickupDirectory);
                return response;
            }

            using var client = new SmtpClient();
            if (_smtpClientOptions.SocketOptions.HasValue)
            {
                await client.ConnectAsync(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.SocketOptions.Value,
                    token.GetValueOrDefault());
            }
            else
            {
                await client.ConnectAsync(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.UseSsl,
                    token.GetValueOrDefault());
            }

            // Note: only needed if the SMTP server requires authentication
            if (_smtpClientOptions.RequiresAuthentication)
            {
                await client.AuthenticateAsync(_smtpClientOptions.User, _smtpClientOptions.Password,
                    token.GetValueOrDefault());
            }

            await client.SendAsync(message, token.GetValueOrDefault());
            await client.DisconnectAsync(true, token.GetValueOrDefault());
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Saves email to a pickup directory.
    /// </summary>
    /// <param name="message">Message to save for pickup.</param>
    /// <param name="pickupDirectory">Pickup directory.</param>
    private async Task SaveToPickupDirectory(MimeMessage message, string pickupDirectory)
    {
        // Note: this will require that you know where the specified pickup directory is.
        var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");

        if (File.Exists(path))
        {
            return;
        }

        await using var stream = new FileStream(path, FileMode.CreateNew);
        await message.WriteToAsync(stream);
    }

    /// <summary>
    /// Create a MimMessage so MailKit can send it
    /// </summary>
    /// <returns>The mail message.</returns>
    /// <param name="email">Email data.</param>
    private MimeMessage CreateMailMessage(EmailData email)
    {
        var message = new MimeMessage();

        // fixes https://github.com/lukencode/FluentEmail/issues/228
        // if for any reason, subject header is not added, add it else update it.
        if (!message.Headers.Contains(HeaderId.Subject))
            message.Headers.Add(HeaderId.Subject, Encoding.UTF8, email.Subject ?? string.Empty);
        else
            message.Headers[HeaderId.Subject] = email.Subject ?? string.Empty;

        message.Headers.Add(HeaderId.Encoding, Encoding.UTF8.EncodingName);

        message.From.Add(new MailboxAddress(email.FromAddress.Name, email.FromAddress.EmailAddress));

        var builder = new BodyBuilder();
        if (!string.IsNullOrEmpty(email.PlaintextAlternativeBody))
        {
            builder.TextBody = email.PlaintextAlternativeBody;
            builder.HtmlBody = email.Body;
        }
        else if (!email.IsHtml)
        {
            builder.TextBody = email.Body;
        }
        else
        {
            builder.HtmlBody = email.Body;
        }

        email.Attachments.ForEach(x =>
        {
            var attachment = builder.Attachments.Add(x.Filename, x.Data, ContentType.Parse(x.ContentType));
            attachment.ContentId = x.ContentId;
        });


        message.Body = builder.ToMessageBody();

        foreach (var header in email.Headers)
        {
            message.Headers.Add(header.Key, header.Value);
        }

        email.ToAddresses.ForEach(x => { message.To.Add(new MailboxAddress(x.Name, x.EmailAddress)); });
        email.CcAddresses.ForEach(x => { message.Cc.Add(new MailboxAddress(x.Name, x.EmailAddress)); });
        email.BccAddresses.ForEach(x => { message.Bcc.Add(new MailboxAddress(x.Name, x.EmailAddress)); });
        email.ReplyToAddresses.ForEach(x => { message.ReplyTo.Add(new MailboxAddress(x.Name, x.EmailAddress)); });
        
        message.Priority = email.Priority switch
        {
            Priority.Low => MessagePriority.NonUrgent,
            Priority.Normal => MessagePriority.Normal,
            Priority.High => MessagePriority.Urgent,
            _ => message.Priority
        };

        return message;
    }
}