#nullable disable

using System.Text;
using Hotboil.Mailer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Hotboil.Areas.Auth.Pages;

public class SignUpConfirmationModel(UserManager<IdentityUser> userManager) : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }
}