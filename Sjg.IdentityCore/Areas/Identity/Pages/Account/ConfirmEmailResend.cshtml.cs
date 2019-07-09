using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;
using Sjg.IdentityCore.Utilities.Mail;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailResendModel : PageModel
    {
        private readonly SignInManager<AccAuthUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IAccAuthConfiguration _accAuthConfiguration;
        private readonly IAccAuthEmailSender _accAuthEmailSender;
        private readonly IAccAuthViewRenderService _accAuthViewRenderService;

        public ConfirmEmailResendModel(
            SignInManager<AccAuthUser> signInManager,
            ILogger<LoginModel> logger,
            IAccAuthConfiguration accAuthConfiguration,
            IAccAuthViewRenderService accAuthViewRenderService,
            IAccAuthEmailSender accAuthEmailSender)
        {
            _signInManager = signInManager;
            _logger = logger;
            _accAuthConfiguration = accAuthConfiguration;
            _accAuthViewRenderService = accAuthViewRenderService;
            _accAuthEmailSender = accAuthEmailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Confirm email")]
            [Compare("Email", ErrorMessage = "The email and confirmation do not match.")]
            public string ConfirmEmail { get; set; }
        }

        //public async Task OnGetAsync()
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var accessUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                if (accessUser == null)
                {
                    _logger.LogWarning($"{Input.Email} Unknown User");
                    ModelState.AddModelError(string.Empty, "Unknown User");
                    return Page();
                }

                // Email Sender - Confirm Email Resend

                var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(accessUser);

                var callbackUri = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = accessUser.Id, code },
                            protocol: Request.Scheme);

                var model = new Views.Email.AccAuth.AccAuthSendVerificationEmailViewModel
                {
                    CallBackUrl = callbackUri.ToString(),
                    Code = code,
                    Email = accessUser.Email,
                    ApplicationName = _accAuthConfiguration.SiteTitle
                };

                var html = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthSendVerificationEmail", model);
                var plain = _accAuthViewRenderService.RenderToStringAsync(@"Email/AccAuth/AccAuthSendVerificationEmail.text", model);

                var emailMessage = new AccAuthEmailMessage
                {
                    Subject = "Confirm Your Account / Email",
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

                return RedirectToPage("./ConfirmEmailSent");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}