namespace Hotboil.Mailer;

public record Address(string EmailAddress, string? Name = null)
{
    public string? Name { get; set; } = Name;
    public string EmailAddress { get; set; } = EmailAddress;

    public Address(string emailAddress) : this(emailAddress, null)
    {
    }
}
