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
    public class RoleGroupsModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;

        public RoleGroupsModel(AccAuthContext accAuthContext)
        {
            _accAuthCtx = accAuthContext;

            GroupSort = "group_asc";
            CatSort = "cat_asc";
            DescrSort = "descr_asc";

            GridPagerModel = new AccAuthGridPagerModel();
        }

        public string GroupSort { get; set; }
        public string CatSort { get; set; }
        public string DescrSort { get; set; }
        public List<AccAuthGroup> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

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

            if (removeId != null)
            {
                var removeItem = _accAuthCtx.AccAuthGroupRoles
                    .Where(o => o.AccAuthGroupId == removeId.Value && o.AccessRoleId == id.Value).FirstOrDefaultAsync().Result;

                if (removeItem != null)
                {
                    _accAuthCtx.AccAuthGroupRoles.Remove(removeItem);
                    var x = _accAuthCtx.SaveChangesAsync().Result;
                }

                return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            }

            Id = id.Value;
            StatusMessage = string.Empty;

            SearchFor = searchFor;
            SortOrder = sortOrder;

            GridPagerModel.Grid_Page = grid_Page;
            GridPagerModel.Grid_Pagesize = grid_Pagesize;
            GridPagerModel.Grid_Buttoncount = grid_Buttoncount;

            IQueryable<AccAuthGroup> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _accAuthCtx.AccAuthGroupRoles.Where(o => o.AccessRoleId == id).Select(o => o.AccAuthGroup);
            }
            else
            {
                qry = _accAuthCtx.AccAuthGroupRoles.Where(o => o.AccAuthGroupId == id &&
                      (o.AccessRole.Name.StartsWith(SearchFor)
                    || o.AccessRole.Category.StartsWith(SearchFor)
                    || o.AccessRole.Description.StartsWith(SearchFor))).Select(o => o.AccAuthGroup);
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

            if (string.IsNullOrWhiteSpace(SortOrder))
            {
                SortOrder = string.Empty;
            }
            else
            {
                SortOrder = SortOrder.Trim().ToLower();
            }

            switch (SortOrder) // lowercase
            {
                case "group_desc":
                    qry = qry.OrderByDescending(s => s.Group);
                    GroupSort = "group_asc";
                    break;

                case "group_asc":
                    qry = qry.OrderBy(s => s.Group);
                    GroupSort = "group_desc";
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
                    qry = qry.OrderBy(s => s.Group);
                    GroupSort = "group_asc";
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