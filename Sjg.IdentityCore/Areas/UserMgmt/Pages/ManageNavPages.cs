using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string RoleMgmt => "RoleMgmt";
        public static string GroupMgmt => "GroupMgmt";
        public static string InvitationMgmt => "InvitationMgmt";
        public static string ConfigMgmt => "ConfigMgmt";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string RoleMgmtNavClass(ViewContext viewContext) => PageNavClass(viewContext, RoleMgmt);

        public static string GroupMgmtNavClass(ViewContext viewContext) => PageNavClass(viewContext, GroupMgmt);

        public static string InvitationMgmtNavClass(ViewContext viewContext) => PageNavClass(viewContext, InvitationMgmt);

        public static string ConfigMgmtNavClass(ViewContext viewContext) => PageNavClass(viewContext, ConfigMgmt);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}