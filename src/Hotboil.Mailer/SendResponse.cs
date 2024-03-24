namespace Hotboil.Mailer;

public class SendResponse
{
    public string? MessageId { get; set; }
    public string? Response { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Comment { get; set; }
    public bool Success => ErrorMessage == null;
}