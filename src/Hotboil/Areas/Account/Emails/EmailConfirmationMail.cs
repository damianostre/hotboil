using Hotboil.Mailer;

namespace Hotboil.Areas.Account.Emails;

public class EmailConfirmationMail : Mail<EmailConfirmationMail>
{
    public string ConfirmationLink { get; set; }
    
    public override string GetSubject() => "Confirm your email";

    public override EmailTemplateInfo GetContent()
    {
        return new FileEmailTemplateInfo
        {
            Path = "EmailConfirmation.html",
        };
    
    }
}

public class Test
{
    public Test()
    {
        var email = new EmailConfirmationMail();
        email.To("ss");
    }
}