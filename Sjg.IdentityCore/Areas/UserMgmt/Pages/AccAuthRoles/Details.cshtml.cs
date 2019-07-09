using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using System;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthRoles
{
    public class DetailsModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;

        public DetailsModel(AccAuthContext accAuthContext)
        {
            _accAuthCtx = accAuthContext;
        }

        public AccAuthRole AccessRole { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccessRole = await _accAuthCtx.AccessRoles.FirstOrDefaultAsync(m => m.Id == id);

            if (AccessRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}