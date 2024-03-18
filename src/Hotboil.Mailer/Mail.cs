namespace Hotboil.Mailer;

public abstract class Mail<T> where T : Mail<T>, new()
{
    private EmailData Data { get; } = new();
    
    public virtual Address? GetFromAddress() => null;
    public abstract string GetSubject();
    public virtual MailContent? GetHtmlContent() => null;
    public virtual MailContent? GetTextContent() => null;
    
    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> To(IEnumerable<string> mailAddresses)
    {
        return To(mailAddresses.ToArray());
    }

    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> To(params string[] mailAddresses)
    {
        foreach (var address in mailAddresses)
        {
            Data.ToAddresses.Add(new Address(address));
        }

        return this;
    }

    /// <summary>
    /// Adds all recipients in list to email
    /// </summary>
    /// <param name="mailAddresses">List of recipients</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> To(IEnumerable<Address> mailAddresses)
    {
        foreach (var address in mailAddresses)
        {
            Data.ToAddresses.Add(address);
        }

        return this;
    }

    /// <summary>
    /// Adds a Carbon Copy to the email
    /// </summary>
    /// <param name="emailAddress">Email address to cc</param>
    /// <param name="name">Name to cc</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> CC(string emailAddress, string name = "")
    {
        Data.CcAddresses.Add(new Address(emailAddress, name));
        return this;
    }

    /// <summary>
    /// Adds all Carbon Copy in list to an email
    /// </summary>
    /// <param name="mailAddresses">List of recipients to CC</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> CC(IEnumerable<Address> mailAddresses)
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
    public Mail<T> BCC(string emailAddress, string name = "")
    {
        Data.BccAddresses.Add(new Address(emailAddress, name));
        return this;
    }

    /// <summary>
    /// Adds all blind carbon copy in list to an email
    /// </summary>
    /// <param name="mailAddresses">List of recipients to BCC</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> BCC(IEnumerable<Address> mailAddresses)
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
    public Mail<T> ReplyTo(string address)
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
    public Mail<T> ReplyTo(string address, string name)
    {
        Data.ReplyToAddresses.Add(new Address(address, name));

        return this;
    }

    /// <summary>
    /// Sets the subject of the email
    /// </summary>
    /// <param name="subject">email subject</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> Subject(string subject)
    {
        Data.Subject = subject;
        return this;
    }

    /// <summary>
    /// Marks the email as High Priority
    /// </summary>
    public Mail<T> HighPriority()
    {
        Data.Priority = Priority.High;
        return this;
    }

    /// <summary>
    /// Marks the email as Low Priority
    /// </summary>
    public Mail<T> LowPriority()
    {
        Data.Priority = Priority.Low;
        return this;
    }

    /// <summary>
    /// Adds an Attachment to the Email
    /// </summary>
    /// <param name="attachment">The Attachment to add</param>
    /// <returns>Instance of the Email class</returns>
    public Mail<T> Attach(Attachment attachment)
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
    public Mail<T> Attach(IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments.Where(attachment => !Data.Attachments.Contains(attachment)))
        {
            Data.Attachments.Add(attachment);
        }
        return this;
    }

    public Mail<T> AttachFromFilename(string filename,  string contentType = null, string attachmentName = null)
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
    public Mail<T> Tag(string tag)
    {
        Data.Tags.Add(tag);

        return this;
    }

    public Mail<T> Header(string header, string body)
    {
        Data.Headers.Add(header, body);

        return this;
    }
    
    public EmailData GetEmailData()
    {
        return Data;
    }
}