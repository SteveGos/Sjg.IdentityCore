using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthRoles
{
    public class RoleUsersModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;
        private readonly UserManager<AccAuthUser> _userManager;

        public RoleUsersModel(AccAuthContext accAuthContext, UserManager<AccAuthUser> userManager)
        {
            _accAuthCtx = accAuthContext;
            _userManager = userManager;

            EmailSort = "email_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthUser>();
        }

        public string EmailSort { get; set; }

        public List<AccAuthUser> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        public Guid Id { get; set; }

        public AccAuthRole AccessRole { get; set; }

        public IActionResult OnGet(Guid? id, Guid? removeId, string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        {
            if (id == null)
            {
                return NotFound(); // 404 Page
            }

            AccessRole = _accAuthCtx.AccessRoles.FirstOrDefault(o => o.Id == id);
            if (AccessRole == null)
            {
                return NotFound(); // 404 Page
            }

            Id = id.Value;

            SearchFor = searchFor;
            SortOrder = sortOrder;

            GridPagerModel.Grid_Page = grid_Page;
            GridPagerModel.Grid_Pagesize = grid_Pagesize;
            GridPagerModel.Grid_Buttoncount = grid_Buttoncount;

            if (string.IsNullOrWhiteSpace(SortOrder))
            {
                SortOrder = string.Empty;
            }
            else
            {
                SortOrder = SortOrder.Trim().ToLower();
            }

            if (removeId != null)
            {
                var usr = _accAuthCtx.Users.FirstOrDefault(o => o.Id == removeId);

                if (usr != null)
                {
                    var result = _userManager.RemoveFromRoleAsync(usr, AccessRole.Name).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
                    }

                    ModelState.AddModelError(string.Empty, "Failed removing user from role");

                    return Page();
                }

                return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            }

            IQueryable<AccAuthUser> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _accAuthCtx.Users  // source
                        .Join(_accAuthCtx.UserRoles, // target
                            u => u.Id,  // FK
                            ur => ur.UserId,  // PK
                            (u, ur) => new { User = u, UserRole = ur }) // projection result
                        .Where(o => o.UserRole.RoleId == id)
                        .Select(x => x.User);  // select result
            }
            else
            {
                qry = _accAuthCtx.Users  // source
                        .Join(_accAuthCtx.UserRoles, // target
                            u => u.Id,  // FK
                            ur => ur.UserId,  // PK
                            (u, ur) => new { User = u, UserRole = ur }) // projection result
                        .Where(o => o.UserRole.RoleId == id && o.User.Email.StartsWith(SearchFor))
                        .Select(x => x.User);  // select result
            }

            var totalRecordsTask = qry.CountAsync();

            if (GridPagerModel.Grid_Pagesize < 1)
            {
                GridPagerModel.Grid_Pagesize = 10;
            }
            if (GridPagerModel.Grid_Page < 1)
            {
                GridPagerModel.Grid_Page = 1;
            }

            switch (SortOrder) // lowercase
            {
                case "email_desc":
                    qry = qry.OrderByDescending(s => s.Email);
                    EmailSort = "email_asc";
                    break;

                case "email_asc":
                    qry = qry.OrderBy(s => s.Email);
                    EmailSort = "email_desc";
                    break;

                default:
                    qry = qry.OrderBy(s => s.Email);
                    EmailSort = "email_asc";
                    break;
            }

            var TotalRecords = totalRecordsTask.Result;

            GridPagerModel.Grid_Pagecount = TotalRecords % GridPagerModel.Grid_Pagesize != 0
                         ? TotalRecords / GridPagerModel.Grid_Pagesize + 1
                         : TotalRecords / GridPagerModel.Grid_Pagesize;

            GridData = qry.AsNoTracking().Skip(GridPagerModel.Grid_Page - 1).Take(GridPagerModel.Grid_Pagesize).ToListAsync().Result;

            return Page();
        }
    }
}