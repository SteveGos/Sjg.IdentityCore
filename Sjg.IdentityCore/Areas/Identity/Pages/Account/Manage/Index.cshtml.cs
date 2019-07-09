using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;
using Sjg.IdentityCore.Utilities.Mail;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AccAuthUser> _userManager;
        private readonly SignInManager<AccAuthUser> _signInManager;
        private readonly IAccAuthConfiguration _accAuthConfiguration;
        private readonly IAccAuthViewRenderService _accAuthViewRenderService;

        public IndexModel(
            UserManager<AccAuthUser> userManager,
            SignInManager<AccAuthUser> signInManager,
            IAccAuthViewRenderService accAuthViewRenderService,
            IAccAuthConfiguration accAuthConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accAuthViewRenderService = accAuthViewRenderService;
            _accAuthConfiguration = accAuthConfiguration;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // SJG: Disable Update....
            ModelState.Clear();
            StatusMessage = "Disabled";
            return await OnGetAsync();

            // Comment out to remove unreachable code message

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //var email = await _userManager.GetEmailAsync(user);
            //if (Input.Email != email)
            //{
            //    var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
            //    if (!setEmailResult.Succeeded)
            //    {
            //        var userId = await _userManager.GetUserIdAsync(user);
            //        throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
            //    }
            //}

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        var userId = await _userManager.GetUserIdAsync(user);
            //        throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            //    }
            //}

            //await _signInManager.RefreshSignInAsync(user);
            //StatusMessage = "Your profile has been updated";
            //// SJG - Identity Fix :: Redirect to Page Clears TempData - Therefore use Page
            //// return RedirectToPage();
            //Username = user.UserName;

            //return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var accessUser = await _userManager.GetUserAsync(User);
            if (accessUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Email Sender - Send Verification Email

            var userId = await _userManager.GetUserIdAsync(accessUser);
            var email = await _userManager.GetEmailAsync(accessUser);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(accessUser);

            var callbackUri = Url.Page(
                           "/Account/ConfirmEmail",
                           pageHandler: null,
                           values: new { userId, code },
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
                Subject = "Confirm Your Email",
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

            StatusMessage = "Verification email sent. Please check your email.";

            Username = accessUser.UserName;

            return Page();
        }
    }
}