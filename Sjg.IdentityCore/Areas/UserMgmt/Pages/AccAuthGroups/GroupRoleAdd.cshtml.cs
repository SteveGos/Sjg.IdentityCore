using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System;
using System.Collections.Generic;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthGroups
{
    public class GroupRoleAddModel : PageModel
    {
        private readonly AccAuthContext _context;

        public GroupRoleAddModel(AccAuthContext context)
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

        public AccAuthGroup AccAuthGroup { get; set; }

        //public IActionResult OnGet(Guid? id, Guid? addId, string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        public IActionResult OnGet()
        {
            //if (id == null)
            //{
            //    return NotFound(); // 404 Page
            //}

            //AccAuthGroup = _context.AccAuthGroups.FirstOrDefault(o => o.AccAuthGroupId == id);
            //if (AccAuthGroup == null)
            //{
            //    return NotFound(); // 404 Page
            //}

            //Id = id.Value;

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

            //if (addId != null)
            //{
            //    if (!_context.AccAuthGroupRoles.Where(o => o.AccAuthGroupId == id.Value && o.AccessRoleId == addId.Value).AnyAsync().Result)
            //    {
            //        _context.AccAuthGroupRoles.Add(new AccAuthGroupRole { AccAuthGroupId = id.Value, AccessRoleId = addId.Value });
            //        var x = _context.SaveChangesAsync().Result;
            //    }

            //    return RedirectToPage(new { id, searchFor, SortOrder, grid_Page, grid_Pagesize, grid_Buttoncount });
            //}

            //// Perform left outer joins
            //// https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-left-outer-joins

            //var curGroupRoles = _context.AccAuthGroupRoles.Where(o => o.AccAuthGroupId == id);

            //var qry = from accessRole in _context.AccessRoles
            //          join AccAuthGroupRole in curGroupRoles on accessRole equals AccAuthGroupRole.AccessRole into joinSet
            //          from item in joinSet.DefaultIfEmpty()
            //          where item.AccAuthGroup == null
            //          select accessRole;

            //if (!string.IsNullOrWhiteSpace(SearchFor))
            //{
            //    qry = qry.Where(o => o.Name.StartsWith(SearchFor)
            //                      || o.Category.StartsWith(SearchFor)
            //                      || o.Description.StartsWith(SearchFor));
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
            //    case "name_desc":
            //        qry = qry.OrderByDescending(s => s.Name);
            //        NameSort = "name_asc";
            //        break;

            //    case "name_asc":
            //        qry = qry.OrderBy(s => s.Name);
            //        NameSort = "name_desc";
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
            //        qry = qry.OrderBy(s => s.Name);
            //        NameSort = "name_asc";
            //        break;
            //}

            //var TotalRecords = totalRecordsTask.Result;

            //GridPagerModel.Grid_Pagecount = TotalRecords % GridPagerModel.Grid_Pagesize != 0
            //             ? TotalRecords / GridPagerModel.Grid_Pagesize + 1
            //             : TotalRecords / GridPagerModel.Grid_Pagesize;

            //GridData = qry.AsNoTracking().Skip(GridPagerModel.Grid_Page - 1).Take(GridPagerModel.Grid_Pagesize).ToListAsync().Result;

            //return Page();

            return RedirectToPage("../Index"); // SJG - Groups Not Implemented
        }
    }
}