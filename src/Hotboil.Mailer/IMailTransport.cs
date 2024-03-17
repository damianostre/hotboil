using FluentEmail.Core.Models;

namespace Hotboil.Mailer;

public interface IMailTransport
{
    SendResponse Send(EmailData email, CancellationToken? token = null);
    Task<SendResponse> SendAsync(EmailData email, CancellationToken? token = null);
}