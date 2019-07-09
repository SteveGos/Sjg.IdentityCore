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
    public class RoleUserAddModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;
        private readonly UserManager<AccAuthUser> _userManager;

        public RoleUserAddModel(AccAuthContext accAuthContext, UserManager<AccAuthUser> userManager)
        {
            _accAuthCtx = accAuthContext;
            _userManager = userManager;

            MailSort = "mail_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthUser>();
        }

        public string MailSort { get; set; }
        public List<AccAuthUser> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        public Guid Id { get; set; }

        public AccAuthRole AccessRole { get; set; }

        public IActionResult OnGet(
            Guid? id,
            Guid? addId,
            string searchFor,
            string sortOrder,
            int grid_Page = 1,
            int grid_Pagesize = 10,
            int grid_Buttoncount = 5)
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

            if (addId != null)
            {
                var usr = _accAuthCtx.Users.FirstOrDefault(o => o.Id == addId);

                if (usr != null)
                {
                    var result = _userManager.AddToRoleAsync(usr, AccessRole.Name).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
                    }

                    ModelState.AddModelError(string.Empty, "Failed adding user to role");

                    return Page();
                }

                return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            }

            // Perform left outer joins
            // https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-left-outer-joins

            //var curRoleUsers = context.UserRoles.Where(o => o.RoleId == AccessRole.Id);

            var curRoleUsers = _accAuthCtx.UserRoles.Where(o => o.RoleId == AccessRole.Id);

            var qry = from usr in _accAuthCtx.Users
                      join userRole in curRoleUsers on usr.Id equals userRole.UserId into joinSet
                      from item in joinSet.DefaultIfEmpty()
                      where item.RoleId == null
                      select usr;

            if (!string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = qry.Where(o => o.Email.StartsWith(SearchFor));
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
                    MailSort = "email_asc";
                    break;

                case "email_asc":
                    qry = qry.OrderBy(s => s.Email);
                    MailSort = "email_desc";
                    break;

                default:
                    qry = qry.OrderBy(s => s.Email);
                    MailSort = "email_asc";
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