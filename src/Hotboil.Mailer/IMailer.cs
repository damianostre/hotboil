namespace Hotboil.Mailer;

public class Mailer : IMailer
{
    public IMailer To(params string[] to)
    {
        return this;
    }
    
    public Task SendAsync<T>(T model) where T: EmailModel
    {
        throw new NotImplementedException();
    }
} 

public interface IMailer
{
    
}