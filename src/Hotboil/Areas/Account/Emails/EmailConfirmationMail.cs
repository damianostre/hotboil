using Hotboil.Mailer;

namespace Hotboil.Areas.Account.Emails;

public class EmailConfirmationMail : Mail<EmailConfirmationMail>
{
    public string ConfirmationLink { get; set; }
    
    public override string GetSubject() => "Confirm your email";

    public override MailContent? GetTextContent()
    {
        return new StringMailContent
        {
            Content = $"Please confirm your email by clicking this link: {ConfirmationLink}"
        };
    }
}