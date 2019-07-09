using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using System;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthInvites
{
    public class DeleteModel : PageModel
    {
        private readonly AccAuthContext _context;

        public DeleteModel(AccAuthContext context)
        {
            _context = context;
        }

        public AccAuthInvite AccAuthInvite { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccAuthInvite = await _context.AccAuthInvites.FirstOrDefaultAsync(m => m.AccAuthInviteId == id);

            if (AccAuthInvite == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccAuthInvite = await _context.AccAuthInvites.FindAsync(id);

            if (AccAuthInvite != null)
            {
                _context.AccAuthInvites.Remove(AccAuthInvite);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../InvitationMgmt");
        }
    }
}