using FluentEmail.Core.Models;

namespace Hotboil.Mailer;

public class MailerMail
{
    protected EmailData Data { get; } = new();

    public MailerMail()
    {
    }
    
    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public static MailerMail To(IEnumerable<string> mailAddresses)
    {
        return To(mailAddresses.ToArray());
    }

    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public static MailerMail To(params string[] mailAddresses)
    {
        var email = new MailerMail();
        foreach (var address in mailAddresses)
        {
            email.Data.ToAddresses.Add(new Address(address));
        }

        return email;
    }

    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public static MailerMail To(IEnumerable<Address> mailAddresses)
    {
        var email = new MailerMail();
        foreach (var address in mailAddresses)
        {
            email.Data.ToAddresses.Add(address);
        }

        return email;
    }

    /// <summary>
    /// Adds a Carbon Copy to the email
    /// </summary>
    /// <param name="emailAddress">Email address to cc</param>
    /// <param name="name">Name to cc</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail CC(string emailAddress, string name = "")
    {
        Data.CcAddresses.Add(new Address(emailAddress, name));
        return this;
    }

    /// <summary>
    /// Adds all Carbon Copy in list to an email
    /// </summary>
    /// <param name="mailAddresses">List of recipients to CC</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail CC(IEnumerable<Address> mailAddresses)
    {
        foreach (var address in mailAddresses)
        {
            Data.CcAddresses.Add(address);
        }
        return this;
    }

    /// <summary>
    /// Adds a blind carbon copy to the email
    /// </summary>
    /// <param name="emailAddress">Email address of bcc</param>
    /// <param name="name">Name of bcc</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail BCC(string emailAddress, string name = "")
    {
        Data.BccAddresses.Add(new Address(emailAddress, name));
        return this;
    }

    /// <summary>
    /// Adds all blind carbon copy in list to an email
    /// </summary>
    /// <param name="mailAddresses">List of recipients to BCC</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail BCC(IEnumerable<Address> mailAddresses)
    {
        foreach (var address in mailAddresses)
        {
            Data.BccAddresses.Add(address);
        }
        return this;
    }

    /// <summary>
    /// Sets the ReplyTo address on the email
    /// </summary>
    /// <param name="address">The ReplyTo Address</param>
    /// <returns></returns>
    public MailerMail ReplyTo(string address)
    {
        Data.ReplyToAddresses.Add(new Address(address));

        return this;
    }

    /// <summary>
    /// Sets the ReplyTo address on the email
    /// </summary>
    /// <param name="address">The ReplyTo Address</param>
    /// <param name="name">The Display Name of the ReplyTo</param>
    /// <returns></returns>
    public MailerMail ReplyTo(string address, string name)
    {
        Data.ReplyToAddresses.Add(new Address(address, name));

        return this;
    }

    /// <summary>
    /// Sets the subject of the email
    /// </summary>
    /// <param name="subject">email subject</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail Subject(string subject)
    {
        Data.Subject = subject;
        return this;
    }

    /// <summary>
    /// Adds a Body to the Email
    /// </summary>
    /// <param name="body">The content of the body</param>
    /// <param name="isHtml">True if Body is HTML, false for plain text (default)</param>
    public MailerMail Body(string body, bool isHtml = false)
    {
        Data.IsHtml = isHtml;
        Data.Body = body;
        return this;
    }

    /// <summary>
    /// Adds a Plaintext alternative Body to the Email. Used in conjunction with an HTML email,
    /// this allows for email readers without html capability, and also helps avoid spam filters.
    /// </summary>
    /// <param name="body">The content of the body</param>
    public MailerMail PlaintextAlternativeBody(string body)
    {
        Data.PlaintextAlternativeBody = body;
        return this;
    }

    /// <summary>
    /// Marks the email as High Priority
    /// </summary>
    public MailerMail HighPriority()
    {
        Data.Priority = Priority.High;
        return this;
    }

    /// <summary>
    /// Marks the email as Low Priority
    /// </summary>
    public MailerMail LowPriority()
    {
        Data.Priority = Priority.Low;
        return this;
    }

    /// <summary>
    /// Adds an Attachment to the Email
    /// </summary>
    /// <param name="attachment">The Attachment to add</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail Attach(Attachment attachment)
    {
        if (!Data.Attachments.Contains(attachment))
        {
            Data.Attachments.Add(attachment);
        }

        return this;
    }

    /// <summary>
    /// Adds Multiple Attachments to the Email
    /// </summary>
    /// <param name="attachments">The List of Attachments to add</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail Attach(IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments.Where(attachment => !Data.Attachments.Contains(attachment)))
        {
            Data.Attachments.Add(attachment);
        }
        return this;
    }

    public MailerMail AttachFromFilename(string filename,  string contentType = null, string attachmentName = null)
    {
        var stream = File.OpenRead(filename);
        Attach(new Attachment
        {
            Data = stream,
            Filename = attachmentName ?? filename,
            ContentType = contentType
        });

        return this;
    }

    /// <summary>
    /// Adds tag to the Email. This is currently only supported by the Mailgun and SendGrid providers. <see href="https://documentation.mailgun.com/en/latest/user_manual.html#tagging"/> and <see href="https://docs.sendgrid.com/for-developers/sending-email/categories"/>
    /// </summary>
    /// <param name="tag">Tag name, max 128 characters, ASCII only</param>
    /// <returns>Instance of the Email class</returns>
    public MailerMail Tag(string tag)
    {
        Data.Tags.Add(tag);

        return this;
    }

    public MailerMail Header(string header, string body)
    {
        Data.Headers.Add(header, body);

        return this;
    }
}