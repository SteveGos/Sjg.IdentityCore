using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;
using Sjg.IdentityCore.Utilities.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthInvites
{
    public class CreateModel : PageModel
    {
        private readonly AccAuthContext _context;
        private readonly IAccAuthViewRenderService _accAuthViewRenderService;
        private readonly IAccAuthConfiguration _accAuthConfiguration;

        public CreateModel(
            AccAuthContext context,
            IAccAuthViewRenderService accAuthViewRenderService,
            IAccAuthConfiguration accAuthConfiguration)
        {
            _context = context;
            _accAuthViewRenderService = accAuthViewRenderService;
            _accAuthConfiguration = accAuthConfiguration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AccAuthInvite AccAuthInvite { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_context.AccAuthInvites.Any(o => o.Email == AccAuthInvite.Email))
            {
                ModelState.AddModelError(string.Empty, "Invitation already exists.");
                return Page();
            }

            if (_accAuthConfiguration.InvitationExpirationDays < 1)
            {
                AccAuthInvite.ExpirationDateUtc = DateTime.UtcNow.AddDays(AccAuthConfiguration.DefaultInvitationExpirationDays);
            }
            else
            {
                AccAuthInvite.ExpirationDateUtc = DateTime.UtcNow.AddDays(_accAuthConfiguration.InvitationExpirationDays);
            }

            AccAuthInvite.Code = $"{System.IO.Path.GetRandomFileName()}{System.IO.Path.GetRandomFileName()}";
            AccAuthInvite.Code = AccAuthInvite.Code.Replace(".", string.Empty).Substring(0, 12).ToUpper();

            _context.AccAuthInvites.Add(AccAuthInvite);
            await _context.SaveChangesAsync();

            // https://Host/Identity/Account/Register

            // https://testapps.itd.idaho.gov/apps/qasp

            // Url.Page --> Wont Resolve {"~/Identity/Account/Register","/Identity/Account/Register", or "/Account/Register"

            // Resolve with New URI:
            var appFolderPath = _accAuthConfiguration.AppFolderPath;  // "/apps/qasp" .... NEED TO DETERMINE PROPER WAY IN CORE TO RESOLVE

            var callbackUri = new Uri($"{Request.Scheme}://{Request.Host.Value}{appFolderPath}/Identity/Account/Register?inviteCode={AccAuthInvite.Code}");
            // var callbackUri = new Uri($"{Request.Scheme}://{Request.Host.Value}/Identity/Account/Register?inviteCode={AccAuthInvite.Code}");

            //// This resolves URL but not "apps/qasp" part of URL.... NEED TO FIGURE OUT PROPER URL Resolution....
            //var callbackUri = Url.Page(
            //              "/Account/Register",
            //              pageHandler: null,
            //              values: new { area = "Identity", inviteCode = AccAuthInvite.Code },
            //              protocol: Request.Scheme);

            var model = new Views.Email.AccAuth.AccAuthInvitationViewModel
            {
                CallBackUrl = callbackUri.ToString(),
                DisplayName = AccAuthInvite.DisplayName,
                Code = AccAuthInvite.Code,
                Email = AccAuthInvite.Email,
                ApplicationName = _accAuthConfiguration.SiteTitle
            };

            var html = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthInvitation", model);
            var plain = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthInvitation.text", model);

            var emailMessage = new AccAuthEmailMessage
            {
                Subject = "Invitation to Register",
                HtmlBody = html.Result,
                TextBody = plain.Result,
            };

            emailMessage.ToAddresses.Add(new AccAuthEmailAddress
            {
                Address = AccAuthInvite.Email,
                Name = AccAuthInvite.DisplayName
            });

            var _accAuthEmailService = new AccAuthEmailService(_accAuthConfiguration);
            await _accAuthEmailService.SendAsync(emailMessage);

            return RedirectToPage("./Details", new { id = AccAuthInvite.AccAuthInviteId });
        }
    }
}