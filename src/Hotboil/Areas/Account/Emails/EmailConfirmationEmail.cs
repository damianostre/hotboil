using Hotboil.Mailer;

namespace Hotboil.Areas.Account.Emails;

public class EmailConfirmationEmail : Mail<EmailConfirmationEmail>
{
    public override string GetSubject() => "Confirm your email";

    public override EmailTemplateInfo GetTemplate()
    {
        throw new NotImplementedException();
    }
}

public class Test
{
    public Test()
    {
        var email = new EmailConfirmationEmail();
        email.To("ss");
    }
}