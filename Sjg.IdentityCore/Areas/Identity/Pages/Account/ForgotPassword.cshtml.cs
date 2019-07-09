using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;
using Sjg.IdentityCore.Utilities.Mail;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AccAuthUser> _userManager;
        private readonly IAccAuthConfiguration _accAuthConfiguration;
        private readonly IAccAuthEmailSender _accAuthEmailSender;
        private readonly IAccAuthViewRenderService _accAuthViewRenderService;

        public ForgotPasswordModel(
            UserManager<AccAuthUser> userManager,
            IAccAuthConfiguration accAuthConfiguration,
            IAccAuthViewRenderService accAuthViewRenderService,
            IAccAuthEmailSender accAuthEmailSender)
        {
            _userManager = userManager;
            _accAuthConfiguration = accAuthConfiguration;
            _accAuthViewRenderService = accAuthViewRenderService;
            _accAuthEmailSender = accAuthEmailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                if (_accAuthConfiguration.AllowLdap &&
                       Input.Email.EndsWith(_accAuthConfiguration.LdapEmail, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(string.Empty, "Internal passwords cannot be reset with this application.");
                    return Page();
                }

                // Email Sender - Forgot Password Email

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                // https://Host/Identity/Account/Register

                var callbackUri = Url.Page(
                            "/Account/ResetPassword",
                            pageHandler: null,
                            values: new { code },
                            protocol: Request.Scheme);

                var model = new Views.Email.AccAuth.AccAuthForgotPasswordViewModel
                {
                    CallBackUrl = callbackUri.ToString(),
                    Code = code,
                    Email = user.Email,
                    ApplicationName = _accAuthConfiguration.SiteTitle
                };

                var html = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthForgotPassword", model);
                var plain = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthForgotPassword.text", model);

                var emailMessage = new AccAuthEmailMessage
                {
                    Subject = "Forgot Password",
                    HtmlBody = html.Result,
                    TextBody = plain.Result,
                };

                emailMessage.ToAddresses.Add(new AccAuthEmailAddress
                {
                    Address = model.Email,
                    Name = model.Email
                });

                var _accAuthEmailService = new AccAuthEmailService(_accAuthConfiguration);
                await _accAuthEmailService.SendAsync(emailMessage);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}