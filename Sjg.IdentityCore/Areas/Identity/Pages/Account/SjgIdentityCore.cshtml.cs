using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sjg.IdentityCore.ActiveDirectory;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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