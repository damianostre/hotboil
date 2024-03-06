namespace Hotboil.Mailer;

public class Mailer : IMailer
{
    public Task SendEmailAsync(EmailModel emailModel)
    {
        throw new NotImplementedException();
    }
} 

public interface IMailer
{
    
}