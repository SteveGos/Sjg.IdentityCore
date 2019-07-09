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
    public class RoleGroupAddModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;

        public RoleGroupAddModel(AccAuthContext accAuthContext)
        {
            _accAuthCtx = accAuthContext;

            NameSort = "group_asc";
            CatSort = "cat_asc";
            DescrSort = "descr_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthGroup>();
        }

        public string NameSort { get; set; }
        public string CatSort { get; set; }
        public string DescrSort { get; set; }
        public List<AccAuthGroup> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        public Guid Id { get; set; }

        public AccAuthRole AccessRole { get; set; }

        public IActionResult OnGet(Guid? id, Guid? addId,
            string searchFor, string sortOrder,
            int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
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
                if (!_accAuthCtx.AccAuthGroupRoles.Where(o => o.AccessRoleId == id.Value && o.AccAuthGroupId == addId.Value).AnyAsync().Result)
                {
                    _accAuthCtx.AccAuthGroupRoles.Add(new AccAuthGroupRole { AccessRoleId = id.Value, AccAuthGroupId = addId.Value });
                    var x = _accAuthCtx.SaveChangesAsync().Result;
                }

                return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            }

            // Perform left outer joins
            // https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-left-outer-joins

            var curGroupRoles = _accAuthCtx.AccAuthGroupRoles.Where(o => o.AccessRoleId == id);

            var qry = from AccAuthGroup in _accAuthCtx.AccAuthGroups
                      join AccAuthGroupRole in curGroupRoles on AccAuthGroup equals AccAuthGroupRole.AccAuthGroup into joinSet
                      from item in joinSet.DefaultIfEmpty()
                      where item.AccessRole == null
                      select AccAuthGroup;

            if (!string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = qry.Where(o => o.Group.StartsWith(SearchFor)
                                  || o.Category.StartsWith(SearchFor)
                                  || o.Description.StartsWith(SearchFor));
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
                case "group_desc":
                    qry = qry.OrderByDescending(s => s.Group);
                    NameSort = "group_asc";
                    break;

                case "group_asc":
                    qry = qry.OrderBy(s => s.Group);
                    NameSort = "group_desc";
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
                    NameSort = "group_asc";
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