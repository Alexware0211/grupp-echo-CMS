namespace Kvarter.Web.Models;

public class ContactFormViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool ConsentGiven { get; set; }
}
