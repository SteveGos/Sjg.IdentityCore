using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System.Collections.Generic;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public partial class GroupMgmtModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;

        public GroupMgmtModel(AccAuthContext accAuthContext)
        {
            _accAuthCtx = accAuthContext;

            GroupSort = "group_asc";
            CatSort = "cat_asc";
            DescrSort = "descr_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthGroup>();
        }

        public string GroupSort { get; set; }
        public string CatSort { get; set; }
        public string DescrSort { get; set; }
        public List<AccAuthGroup> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }
        public string SortOrder { get; set; }

        public IActionResult OnGet(string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        {
            return RedirectToPage("./Index");

            //SearchFor = searchFor;
            //SortOrder = sortOrder;

            //GridPagerModel.Grid_Page = grid_Page;
            //GridPagerModel.Grid_Pagesize = grid_Pagesize;
            //GridPagerModel.Grid_Buttoncount = grid_Buttoncount;

            //if (string.IsNullOrWhiteSpace(SortOrder))
            //{
            //    SortOrder = string.Empty;
            //}
            //else
            //{
            //    SortOrder = SortOrder.Trim().ToLower();
            //}

            //IQueryable<AccAuthGroup> qry;

            //if (string.IsNullOrWhiteSpace(SearchFor))
            //{
            //    qry = _accAuthCtx.AccAuthGroups;
            //}
            //else
            //{
            //    qry = _accAuthCtx.AccAuthGroups.Where(o => o.Group.StartsWith(SearchFor)
            //                                         || o.Category.StartsWith(SearchFor)
            //                                         || o.Description.StartsWith(SearchFor));
            //}

            //var totalRecordsTask = qry.CountAsync();

            //if (GridPagerModel.Grid_Pagesize < 1)
            //{
            //    GridPagerModel.Grid_Pagesize = 10;
            //}
            //if (GridPagerModel.Grid_Page < 1)
            //{
            //    GridPagerModel.Grid_Page = 1;
            //}

            //switch (SortOrder) // lowercase
            //{
            //    case "group_desc":
            //        qry = qry.OrderByDescending(s => s.Group);
            //        GroupSort = "group_asc";
            //        break;

            //    case "group_asc":
            //        qry = qry.OrderBy(s => s.Group);
            //        GroupSort = "group_desc";
            //        break;

            //    case "cat_desc":
            //        qry = qry.OrderByDescending(s => s.Category);
            //        CatSort = "cat_asc";
            //        break;

            //    case "cat_asc":
            //        qry = qry.OrderBy(s => s.Category);
            //        CatSort = "cat_desc";
            //        break;

            //    case "descr_desc":
            //        qry = qry.OrderByDescending(s => s.Description);
            //        DescrSort = "descr_asc";
            //        break;

            //    case "descr_asc":
            //        qry = qry.OrderBy(s => s.Description);
            //        DescrSort = "descr_desc";
            //        break;

            //    default:
            //        qry = qry.OrderBy(s => s.Group);
            //        GroupSort = "group_asc";
            //        break;
            //}

            //var TotalRecords = totalRecordsTask.Result;

            //GridPagerModel.Grid_Pagecount = TotalRecords % GridPagerModel.Grid_Pagesize != 0
            //             ? TotalRecords / GridPagerModel.Grid_Pagesize + 1
            //             : TotalRecords / GridPagerModel.Grid_Pagesize;

            //GridData = qry.AsNoTracking().Skip(GridPagerModel.Grid_Page - 1).Take(GridPagerModel.Grid_Pagesize).ToListAsync().Result;

            //return Page();
        }
    }
}