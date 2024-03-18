namespace Hotboil.Mailer;

public class EmailData
{
    public IList<Address> ToAddresses { get; set; } = new List<Address>();
    public IList<Address> CcAddresses { get; set; } = new List<Address>();
    public IList<Address> BccAddresses { get; set; } = new List<Address>();
    public IList<Address> ReplyToAddresses { get; set; } = new List<Address>();
    public IList<Attachment> Attachments { get; set; } = new List<Attachment>();
    public Address? FromAddress { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? PlaintextAlternativeBody { get; set; }
    public Priority? Priority { get; set; }
    public IList<string> Tags { get; set; } = new List<string>();

    public bool IsHtml { get; set; } = true;
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
