using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AccAuthUser> _userManager;

        public IndexModel(UserManager<AccAuthUser> userManager)
        {
            _userManager = userManager;

            EmailSort = string.Empty;
            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthUser>();
        }

        public string EmailSort { get; set; }
        public List<AccAuthUser> GridData { get; set; }
        public AccAuthGridPagerModel GridPagerModel { get; set; }

        public string SearchFor { get; set; }

        public void OnGet(string searchFor, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
        {
            SearchFor = searchFor;

            GridPagerModel.Grid_Page = grid_Page;
            GridPagerModel.Grid_Pagesize = grid_Pagesize;
            GridPagerModel.Grid_Buttoncount = grid_Buttoncount;

            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                sortOrder = string.Empty;
            }
            else
            {
                sortOrder = sortOrder.Trim().ToLower();
            }

            IQueryable<AccAuthUser> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _userManager.Users;
            }
            else
            {
                qry = _userManager.Users.Where(o => o.Email.StartsWith(SearchFor));
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

            switch (sortOrder) // lowercase
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
                    EmailSort = "email_desc";
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