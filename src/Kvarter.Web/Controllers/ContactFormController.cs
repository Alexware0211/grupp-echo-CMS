using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Kvarter.Web.Data;
using Kvarter.Web.Data.Entities;
using Kvarter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Kvarter.Web.Controllers;

public class ContactFormController : SurfaceController
{
    private readonly KvarterDbContext _dbContext;
    private readonly ILogger<ContactFormController> _logger;

    public ContactFormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        KvarterDbContext dbContext,
        ILogger<ContactFormController> logger)
        : base(
            umbracoContextAccessor,
            databaseFactory,
            services,
            appCaches,
            profilingLogger,
            publishedUrlProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        [Bind(Prefix = "")] ContactFormViewModel model)
    {
        if (CurrentPage is null)
        {
            return BadRequest();
        }

        var culture = CultureInfo.CurrentUICulture;
        var isSwedish = culture.TwoLetterISOLanguageName == "sv";

        model.Name = (model.Name ?? string.Empty).Trim();
        model.Email = (model.Email ?? string.Empty).Trim();
        model.Subject = (model.Subject ?? string.Empty).Trim();
        model.Message = (model.Message ?? string.Empty).Trim();

        ValidateText(
            nameof(model.Name),
            model.Name,
            100,
            isSwedish ? "Ange ditt namn." : "Enter your name.",
            isSwedish
                ? "Namnet får innehålla högst 100 tecken."
                : "Your name must be 100 characters or fewer.");

        ValidateText(
            nameof(model.Email),
            model.Email,
            256,
            isSwedish ? "Ange din e-postadress." : "Enter your email address.",
            isSwedish
                ? "E-postadressen får innehålla högst 256 tecken."
                : "Your email address must be 256 characters or fewer.");

        if (!string.IsNullOrWhiteSpace(model.Email)
            && model.Email.Length <= 256
            && !new EmailAddressAttribute().IsValid(model.Email))
        {
            ModelState.AddModelError(
                nameof(model.Email),
                isSwedish
                    ? "Ange en giltig e-postadress."
                    : "Enter a valid email address.");
        }

        ValidateText(
            nameof(model.Subject),
            model.Subject,
            200,
            isSwedish ? "Ange ett ämne." : "Enter a subject.",
            isSwedish
                ? "Ämnet får innehålla högst 200 tecken."
                : "The subject must be 200 characters or fewer.");

        ValidateText(
            nameof(model.Message),
            model.Message,
            4000,
            isSwedish ? "Skriv ditt meddelande." : "Enter your message.",
            isSwedish
                ? "Meddelandet får innehålla högst 4000 tecken."
                : "Your message must be 4000 characters or fewer.");

        var consentBindingFailed =
            ModelState[nameof(model.ConsentGiven)]?.Errors.Count > 0;

        ModelState[nameof(model.ConsentGiven)]?.Errors.Clear();

        if (!model.ConsentGiven || consentBindingFailed)
        {
            ModelState.AddModelError(
                nameof(model.ConsentGiven),
                isSwedish
                    ? "Du måste ge ditt samtycke för att skicka meddelandet."
                    : "You must give your consent to send the message.");
        }

        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var contactMessage = new ContactMessage
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message,
            Culture = culture.Name,
            ConsentGiven = model.ConsentGiven,
            IsHandled = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        try
        {
            _dbContext.ContactMessages.Add(contactMessage);
            await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Could not save a contact form submission.");

            ModelState.AddModelError(
                string.Empty,
                isSwedish
                    ? "Meddelandet kunde inte sparas. Försök igen."
                    : "Your message could not be saved. Please try again.");

            return CurrentUmbracoPage();
        }

        TempData["ContactFormSuccess"] = true;

        return RedirectToCurrentUmbracoPage();
    }

    private void ValidateText(
        string fieldName,
        string value,
        int maximumLength,
        string requiredMessage,
        string lengthMessage)
    {
        // Replace automatic required errors with our translated messages.
        ModelState[fieldName]?.Errors.Clear();

        if (string.IsNullOrWhiteSpace(value))
        {
            ModelState.AddModelError(fieldName, requiredMessage);
        }
        else if (value.Length > maximumLength)
        {
            ModelState.AddModelError(fieldName, lengthMessage);
        }
    }
}
