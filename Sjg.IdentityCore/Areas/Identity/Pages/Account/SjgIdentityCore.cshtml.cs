using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [Attributes.ViewLayout("_Layout_Login")]
    public class SjgIdentityCoreModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}