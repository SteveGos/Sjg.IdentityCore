using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthInvites
{
    public class InviteRolesModel : PageModel
    {
        private readonly AccAuthContext _context;

        public InviteRolesModel(AccAuthContext context)
        {
            _context = context;

            NameSort = "name_asc";
            CatSort = "cat_asc";
            DescrSort = "descr_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthRole>();
        }

        public string NameSort { get; set; }
        public string CatSort { get; set; }
        public string DescrSort { get; set; }
        public List<AccAuthRole> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        public Guid Id { get; set; }

        public AccAuthInvite AccAuthInvite { get; set; }

        public IActionResult OnGet(Guid? id, Guid? removeId, string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        {
            if (id == null)
            {
                return NotFound(); // 404 Page
            }

            AccAuthInvite = _context.AccAuthInvites.FirstOrDefault(o => o.AccAuthInviteId == id);
            if (AccAuthInvite == null)
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
                var removeItem = _context.AccAuthInviteRoles
                    .Where(o => o.AccAuthInviteId == id.Value && o.AccessRoleId == removeId.Value).FirstOrDefaultAsync().Result;

                if (removeItem != null)
                {
                    _context.AccAuthInviteRoles.Remove(removeItem);
                    var x = _context.SaveChangesAsync().Result;
                }

                return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            }

            IQueryable<AccAuthRole> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _context.AccAuthInviteRoles.Where(o => o.AccAuthInviteId == id).Select(o => o.AccessRole);
            }
            else
            {
                qry = _context.AccAuthInviteRoles.Where(o => o.AccAuthInviteId == id &&
                      (o.AccessRole.Name.StartsWith(SearchFor)
                    || o.AccessRole.Category.StartsWith(SearchFor)
                    || o.AccessRole.Description.StartsWith(SearchFor))).Select(o => o.AccessRole);
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
                case "name_desc":
                    qry = qry.OrderByDescending(s => s.Name);
                    NameSort = "name_asc";
                    break;

                case "name_asc":
                    qry = qry.OrderBy(s => s.Name);
                    NameSort = "name_desc";
                    break;

                case "cat_desc":
                    qry = qry.OrderByDescending(s => s.Category);
                    CatSort = "cat_asc";
                    break;

                case "cat_asc":
                    qry = qry.OrderBy(s => s.Category);
                    CatSort = "cat_desc";
                    break;

                case "descr_desc":
                    qry = qry.OrderByDescending(s => s.Description);
                    DescrSort = "descr_asc";
                    break;

                case "descr_asc":
                    qry = qry.OrderBy(s => s.Description);
                    DescrSort = "descr_desc";
                    break;

                default:
                    qry = qry.OrderBy(s => s.Name);
                    NameSort = "name_asc";
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