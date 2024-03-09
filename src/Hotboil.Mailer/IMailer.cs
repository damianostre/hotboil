namespace Hotboil.Mailer;

public class Mailer : IMailer
{
    public Task SendAsync<T>(T mail) where T : Mailable<T>, new()
    {
        throw new NotImplementedException();
    }
} 

public interface IMailer
{
    
}