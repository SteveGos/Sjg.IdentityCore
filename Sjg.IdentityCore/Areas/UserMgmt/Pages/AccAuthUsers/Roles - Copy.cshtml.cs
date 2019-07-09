//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using Sjg.IdentityCore.Models;
//using Sjg.IdentityCore.TagHelpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthUsers
//{
//    public class RolesModel : PageModel
//    {
//        //private readonly IServiceProvider _serviceProvider;
//        private readonly UserManager<AccAuthUser> _userManager;

//        private readonly RoleManager<AccAuthRole> _roleManager;

//        private readonly AccAuthContext _accAuthCtx;

//        public RolesModel(AccAuthContext accAuthContext, UserManager<AccAuthUser> userManager, RoleManager<AccAuthRole> roleManager)
//        {
//            _accAuthCtx = accAuthContext;
//            _userManager = userManager;

//            _roleManager = roleManager;

//            NameSort = "name_asc";
//            DescrSort = "descr_asc";

//            GridPagerModel = new AccAuthGridPagerModel();
//            GridData = new List<AccAuthRole>();

//            StatusMessage = string.Empty;
//        }

//        public Guid Id { get; set; }

//        public AccAuthUser AccAuthUser { get; set; }

//        public string NameSort { get; set; }
//        public string DescrSort { get; set; }
//        public List<AccAuthRole> GridData { get; set; }
//        public AccAuthGridPagerModel GridPagerModel { get; set; }

//        public string SortOrder { get; set; }

//        public async Task<IActionResult> OnGet(Guid? id, string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
//        {
//            return await GetData(id, sortOrder, grid_Page, grid_Pagesize, grid_Buttoncount);
//        }

//        private async Task<IActionResult> GetData(Guid? id, string sortOrder, int grid_Page, int grid_Pagesize, int grid_Buttoncount)
//        {
//            if (id == null)
//            {
//                return NotFound(); // 404 Page
//            }

//            AccAuthUser = await _accAuthCtx.Users.FirstOrDefaultAsync(o => o.Id == id);

//            if (AccAuthUser == null)
//            {
//                return NotFound(); // 404 Page
//            }

//            Id = id.Value;
//            StatusMessage = string.Empty;

//            SortOrder = sortOrder;

//            GridPagerModel.Grid_Page = grid_Page;
//            GridPagerModel.Grid_Pagesize = grid_Pagesize;
//            GridPagerModel.Grid_Buttoncount = grid_Buttoncount;

//            if (string.IsNullOrWhiteSpace(SortOrder))
//            {
//                SortOrder = string.Empty;
//            }
//            else
//            {
//                SortOrder = SortOrder.Trim().ToLower();
//            }

//            if (GridPagerModel.Grid_Pagesize < 1)
//            {
//                GridPagerModel.Grid_Pagesize = 10;
//            }
//            if (GridPagerModel.Grid_Page < 1)
//            {
//                GridPagerModel.Grid_Page = 1;
//            }

//            var qry = _accAuthCtx.Roles  // source
//                        .Join(_accAuthCtx.UserRoles, // target
//                            r => r.Id,  // FK
//                            ur => ur.RoleId,  // PK
//                            (r, ur) => new { AccAuthRole = r, AccAuthUserRole = ur }) // projection result
//                        .Where(o => o.AccAuthUserRole.UserId == Id)
//                        .Select(x => x.AccAuthRole);  // select result

//            var totalRecordsTask = qry.CountAsync();

//            switch (SortOrder) // lowercase
//            {
//                case "name_desc":
//                    qry = qry.OrderByDescending(s => s.Name);
//                    NameSort = "name_asc";
//                    break;

//                case "name_asc":
//                    qry = qry.OrderBy(s => s.Name);
//                    NameSort = "name_desc";
//                    break;

//                case "descr_desc":
//                    qry = qry.OrderByDescending(s => s.Description);
//                    DescrSort = "descr_asc";
//                    break;

//                case "descr_asc":
//                    qry = qry.OrderBy(s => s.Description);
//                    DescrSort = "descr_desc";
//                    break;

//                default:
//                    qry = qry.OrderBy(s => s.Name);
//                    NameSort = "name_asc";
//                    break;
//            }

//            var TotalRecords = await totalRecordsTask;

//            GridPagerModel.Grid_Pagecount = TotalRecords % GridPagerModel.Grid_Pagesize != 0
//                         ? TotalRecords / GridPagerModel.Grid_Pagesize + 1
//                         : TotalRecords / GridPagerModel.Grid_Pagesize;

//            GridData = await qry.AsNoTracking().Skip(GridPagerModel.Grid_Page - 1).Take(GridPagerModel.Grid_Pagesize).ToListAsync();

//            return Page();

//            //var roleIdList = _accAuthCtx.UserRoles.Where(o => o.UserId == AccAuthUser.Id).Select(o => o.RoleId).ToList();
//            //if (roleIdList.Any())
//            //{
//            //    GridData = _accAuthCtx.Roles.Where(o => roleIdList.Contains(o.Id)).ToList();
//            //}
//        }

//        public async Task<IActionResult> OnPost(Guid? id, Guid? RoleId,
//            string sortOrder, int grid_Page = 1, int grid_Pagesize = 10, int grid_Buttoncount = 5)
//        {
//            return await GetData(id, sortOrder, grid_Page, grid_Pagesize, grid_Buttoncount);
//        }
//    }
//}