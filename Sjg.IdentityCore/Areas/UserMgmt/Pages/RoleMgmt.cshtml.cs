using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public partial class RoleMgmtModel : PageModel
    {
        private readonly RoleManager<AccAuthRole> _roleManager;

        public RoleMgmtModel(RoleManager<AccAuthRole> roleManager)
        {
            _roleManager = roleManager;

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

        public void OnGet(string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        {
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

            IQueryable<AccAuthRole> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _roleManager.Roles;
            }
            else
            {
                qry = _roleManager.Roles.Where(o => o.Name.StartsWith(SearchFor)
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
        }
    }
}