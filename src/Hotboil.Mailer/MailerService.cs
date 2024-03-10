using FluentEmail.Core;

namespace Hotboil.Mailer;

public class MailerService : IMailerService
{
    public async Task SendAsync<T>(T mail) where T : Mail<T>, new()
    {
        var email = new Email().Data = mail.GetEmailData();
        
            
    }
} 

public interface IMailerService
{
    
}