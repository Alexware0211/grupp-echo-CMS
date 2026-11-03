namespace Kvarter.Web.Data.Entities;

public class NewsletterSubscriber
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Culture { get; set; } = string.Empty;

    public bool ConsentGiven { get; set; }

    public DateTime SubscribedAtUtc { get; set; } = DateTime.UtcNow;
}
