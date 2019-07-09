using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthGroups
{
    public class DetailsModel : PageModel
    {
        private readonly AccAuthContext _context;

        public DetailsModel(AccAuthContext context)
        {
            _context = context;
        }

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
    }
}