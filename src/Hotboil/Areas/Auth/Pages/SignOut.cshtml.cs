#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotboil.Areas.Auth.Pages;

public class SignOutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<SignOutModel> _logger;

    public SignOutModel(SignInManager<IdentityUser> signInManager, ILogger<SignOutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User sign out");

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return Redirect("/");
    }
}