using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailRequiredModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}