using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Net;

// https://getbootstrap.com/docs/4.1/components/pagination/

// <div>
//     <nav aria-label="Page navigation">
//         <ul class="pagination">
//            <li class="page-item"><a class="page-link" href="#">Previous</a></li>
//            <li class="page-item"><a class="page-link" href="#">1</a></li>
//            <li class="page-item active"><a class="page-link" href="#">2</a></li>
//            <li class="page-item"><a class="page-link" href="#">3</a></li>
//            <li class="page-item"><a class="page-link" href="#">Next</a></li>
//        </ul>
//    </nav>
// </div>

namespace Sjg.IdentityCore.TagHelpers
{
    /// <summary>
    /// Generate a Bootstrap 4 Pager...
    /// <acc-auth-grid-pager grid-pagecount="20" grid-page="1" grid-pagesize="10" grid-buttoncount="5" url="ProtoType" ></acc-auth-grid-pager>
    /// </summary>
    public class AccAuthGridPagerTagHelper : TagHelper
    {
        private readonly HttpContext _httpContext;
        private readonly IUrlHelper _urlHelper;

        //////////  Notes:
        //////////       - Pascal-cased class and property names for tag helpers are translated into their lower kebab case.
        //////////         Therefore, to use the GridPagesize attribute, you'll use
        //////////          <acc-auth-grid-pager grid-pagecount="20" grid-page="1" grid-pagesize="10" url="ProtoType" grid-buttoncount="5"></acc-auth-grid-pager> equivalent.

        ////////// To Override Default Naming Scheme
        //////////private const string Grid_PagesizeAttributeName = "grid-pagesize";
        //////////[HtmlAttributeName(Grid_PagesizeAttributeName)]

        /// <summary>
        /// Page size to pass to Data Retrieval
        /// </summary>
        public int Grid_Pagesize { get; set; }

        /// <summary>
        /// Page to retrieve to pass to Data Retrieval
        /// </summary>
        public int Grid_Page { get; set; }

        /// <summary>
        /// Number of pages - from Data Retrieval.
        /// </summary>
        public int Grid_Pagecount { get; set; }

        /// <summary>
        /// Number of page buttons in pager. Must be between 3 and 11 and an odd number.  Defaults to 5.
        /// </summary>
        public int Grid_Buttoncount { get; set; }

        public string Url { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AccAuthGridPagerTagHelper(
            IHttpContextAccessor accessor,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory)
        {
            _httpContext = accessor.HttpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Grid_Pagecount <= 1)
            {
                return;
            }

            switch (Grid_Buttoncount)
            {
                case 3:
                case 5:
                case 7:
                case 9:
                case 11:
                    break;

                default:
                    Grid_Buttoncount = 5;
                    break;
            }

            var action = ViewContext.RouteData.Values["action"] == null ? string.Empty : ViewContext.RouteData.Values["action"].ToString();
            var urlTemplate = WebUtility.UrlDecode(_urlHelper.Action(action,
                    new { grid_page = "{0}", grid_pagesize = $"{Grid_Pagesize}", grid_buttoncount = $"{Grid_Buttoncount}" }));

            var request = _httpContext.Request;
            foreach (var key in request.Query.Keys)
            {
                if (key.Equals("grid_page", StringComparison.OrdinalIgnoreCase) ||
                    key.Equals("grid_pagecount", StringComparison.OrdinalIgnoreCase) ||
                    key.Equals("grid_pagesize", StringComparison.OrdinalIgnoreCase) ||
                    key.Equals("grid_buttoncount", StringComparison.OrdinalIgnoreCase))
                //key.Equals("url", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                urlTemplate += "&" + key + "=" + request.Query[key];
            }

            var btncnt = (Grid_Buttoncount - 1) / 2;

            var startIndex = Grid_Page - btncnt;
            if (startIndex < 1)
            {
                startIndex = 1;
            }

            var finishIndex = startIndex + Grid_Buttoncount - 1;
            if (finishIndex > Grid_Pagecount)
            {
                finishIndex = Grid_Pagecount;
            }

            output.TagName = "";
            output.Content.AppendHtml("<ul class=\"pagination\">");

            if (Grid_Page > 1)
            {
                AddPageLink(output, string.Format(urlTemplate, 1), "&laquo;");
            }

            if (Grid_Page > btncnt + 1)
            {
                AddPageLink(output, string.Format(urlTemplate, startIndex - 1), "...");
            }

            for (var i = startIndex; i <= finishIndex; i++)
            {
                if (i == Grid_Page)
                {
                    AddCurrentPageLink(output, i, i.ToString());
                }
                else
                {
                    AddPageLink(output, string.Format(urlTemplate, i), i.ToString());
                }
            }

            if (Grid_Page < (Grid_Pagecount - btncnt))
            {
                AddPageLink(output, string.Format(urlTemplate, finishIndex + 1), "...");
            }

            if (Grid_Page < Grid_Pagecount)
            {
                AddPageLink(output, string.Format(urlTemplate, Grid_Pagecount), "&raquo;");
            }

            output.Content.AppendHtml("</ul>");
        }

        private void AddPageLink(TagHelperOutput output, string url, string text)
        {
            output.Content.AppendHtml("<li class=\"page-item\"><a class=\"page-link\" href=\"");
            output.Content.AppendHtml(url);
            output.Content.AppendHtml("\">");
            output.Content.AppendHtml(text);
            output.Content.AppendHtml("</a>");
            output.Content.AppendHtml("</li>");
        }

        private void AddCurrentPageLink(TagHelperOutput output, int page, string text)
        {
            output.Content.AppendHtml("<li class=\"page-item active\"><a class=\"page-link\" href=#");
            output.Content.AppendHtml("\">");
            output.Content.AppendHtml(text);
            output.Content.AppendHtml("</a>");
            output.Content.AppendHtml("</li>");
        }
    }
}