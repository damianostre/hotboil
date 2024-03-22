namespace Hotboil.Mailer;

public class EmailData
{
    public List<Address> ToAddresses { get; set; } = new();
    public List<Address> CcAddresses { get; set; } = new();
    public List<Address> BccAddresses { get; set; } = new();
    public List<Address> ReplyToAddresses { get; set; } = new();
    public List<Attachment> Attachments { get; set; } = new();
    public Address? FromAddress { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? PlaintextAlternativeBody { get; set; }
    public Priority? Priority { get; set; }
    public List<string> Tags { get; set; } = new();

    public bool IsHtml { get; set; } = true;
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
