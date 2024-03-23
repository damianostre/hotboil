namespace Hotboil.Mailer;

public class SendResponse
{
    public string? MessageId { get; set; }
    public IList<string> ErrorMessages { get; set; } = new List<string>();
    public bool Successful => !ErrorMessages.Any();
}