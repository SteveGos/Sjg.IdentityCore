using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthGroups
{
    public class EditModel : PageModel
    {
        private readonly AccAuthContext _context;

        public EditModel(AccAuthContext context)
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

        //public async Task<IActionResult> OnPostAsync()
        public IActionResult OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(AccAuthGroup).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AccAuthGroupExists(AccAuthGroup.AccAuthGroupId))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return RedirectToPage("./Details", new { id = AccAuthGroup.AccAuthGroupId });

            return RedirectToPage("../Index"); // SJG - Groups Not Implemented
        }

        //private bool AccAuthGroupExists(Guid id)
        //{
        //    return _context.AccAuthGroups.Any(e => e.AccAuthGroupId == id);
        //}
    }
}