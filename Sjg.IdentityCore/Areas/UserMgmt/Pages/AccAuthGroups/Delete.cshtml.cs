using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthGroups
{
    public class DeleteModel : PageModel
    {
        private readonly AccAuthContext _context;

        public DeleteModel(AccAuthContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccAuthGroup AccAuthGroup { get; set; }

        //public async Task<IActionResult> OnGetAsync(Guid? id)
        public IActionResult OnGet()
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //AccAuthGroup = await _context.AccAuthGroups.FirstOrDefaultAsync(m => m.AccAuthGroupId == id);

            //if (AccAuthGroup == null)
            //{
            //    return NotFound();
            //}
            //return Page();

            return RedirectToPage("../Index"); // SJG - Groups Not Implemented
        }

        //public async Task<IActionResult> OnPostAsync(Guid? id)
        public IActionResult OnPost()
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //AccAuthGroup = await _context.AccAuthGroups.FindAsync(id);

            //if (AccAuthGroup != null)
            //{
            //    _context.AccAuthGroups.Remove(AccAuthGroup);
            //    await _context.SaveChangesAsync();
            //}

            //return RedirectToPage("../GroupMgmt");

            return RedirectToPage("../Index");
        }
    }
}