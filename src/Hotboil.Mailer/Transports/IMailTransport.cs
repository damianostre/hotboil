namespace Hotboil.Mailer.Transports;

public interface IMailTransport
{
    SendResponse Send(EmailData email, CancellationToken? token = null);
    Task<SendResponse> SendAsync(EmailData email, CancellationToken? token = null);
}