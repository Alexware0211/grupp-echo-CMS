namespace Kvarter.Web.Data.Entities;

public class ContactMessage
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Culture { get; set; } = string.Empty;

    public bool ConsentGiven { get; set; }

    public bool IsHandled { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
