using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Services;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages
{
    public class ConfigMgmtModel : PageModel
    {
        public ConfigMgmtModel(
            IAccAuthConfiguration accAuthConfiguration)
        {
            AccAuthConfiguration = accAuthConfiguration;
        }

        public IAccAuthConfiguration AccAuthConfiguration { get; private set; }

        public void OnGet()
        {
        }
    }
}