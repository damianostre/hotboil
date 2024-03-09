using Hotboil.Mailer;

namespace Hotboil.Areas.Account.Emails;

public class EmailConfirmationEmail : Mailable<EmailConfirmationEmail>
{
    
}

public class Test
{
    public Test()
    {
        var email = new EmailConfirmationEmail();
        email.To("ss").Body()
    }
}