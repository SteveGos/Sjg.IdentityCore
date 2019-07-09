using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public partial class InvitationMgmtModel : PageModel
    {
        private readonly AccAuthContext _accAuthCtx;

        public InvitationMgmtModel(AccAuthContext accAuthContext)
        {
            _accAuthCtx = accAuthContext;

            EmailSort = "email_asc";
            ExpirationSort = "expiration_asc";
            DisplayNameSort = "displayname_asc";

            GridPagerModel = new AccAuthGridPagerModel();
            GridData = new List<AccAuthInvite>();
        }

        public string EmailSort { get; set; }
        public string ExpirationSort { get; set; }
        public string DisplayNameSort { get; set; }
        public List<AccAuthInvite> GridData { get; set; }
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

            IQueryable<AccAuthInvite> qry;

            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                qry = _accAuthCtx.AccAuthInvites;
            }
            else
            {
                qry = _accAuthCtx.AccAuthInvites.Where(o => o.Email.StartsWith(SearchFor)
                                                     || o.DisplayName.StartsWith(SearchFor));
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

                case "expiration_desc":
                    qry = qry.OrderByDescending(s => s.ExpirationDateUtc);
                    ExpirationSort = "expiration_asc";
                    break;

                case "expiration_asc":
                    qry = qry.OrderBy(s => s.ExpirationDateUtc);
                    ExpirationSort = "expiration_desc";
                    break;

                case "displayname_desc":
                    qry = qry.OrderByDescending(s => s.DisplayName);
                    DisplayNameSort = "displayname_asc";
                    break;

                case "displayname_asc":
                    qry = qry.OrderBy(s => s.DisplayName);
                    DisplayNameSort = "displayname_desc";
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

            //StatusMessage = $"Recs: {TotalRecords} GridPageCount: {GridPagerModel.Grid_Pagecount} GridPage {GridPagerModel.Grid_Page} GridPageSize; {GridPagerModel.Grid_Pagesize} EmailSort {EmailSort}";
        }
    }
}