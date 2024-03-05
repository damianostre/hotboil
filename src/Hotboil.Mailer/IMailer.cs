namespace Hotboil.Mailer;

public class Mailer : IMailer
{
    public Task SendEmailAsync(IMail mail)
    {
        throw new NotImplementedException();
    }
} 

public interface IMailer
{
    
}