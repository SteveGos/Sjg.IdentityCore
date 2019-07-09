using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthGroups
{
    public class CreateModel : PageModel
    {
        private readonly AccAuthContext _context;

        public CreateModel(AccAuthContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //return Page();

            return RedirectToPage("../Index"); // SJG - Groups Not Implemented
        }

        [BindProperty]
        public AccAuthGroup AccAuthGroup { get; set; }

        //public async Task<IActionResult> OnPostAsync()
        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.AccAuthGroups.Add(AccAuthGroup);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("../GroupMgmt");

            return RedirectToPage("../Index"); // SJG - Groups Not Implemented
        }
    }
}