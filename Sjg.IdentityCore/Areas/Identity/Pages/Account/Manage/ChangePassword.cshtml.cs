using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<AccAuthUser> _userManager;
        private readonly SignInManager<AccAuthUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IAccAuthConfiguration _accAuthConfiguration;

        public ChangePasswordModel(
            UserManager<AccAuthUser> userManager,
            SignInManager<AccAuthUser> signInManager,
            ILogger<ChangePasswordModel> logger,
            IAccAuthConfiguration accAuthConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accAuthConfiguration = accAuthConfiguration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (_accAuthConfiguration.AllowLdap &&
                user.Email.EndsWith(_accAuthConfiguration.LdapEmail, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Internal passwords cannot be reset with this application.");
                return Page();
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (_accAuthConfiguration.AllowLdap &&
                 user.Email.EndsWith(_accAuthConfiguration.LdapEmail, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Internal passwords cannot be reset with this application.");
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            user.LastPasswordChangeDateTimeUtc = DateTime.UtcNow;
            var savePasswordResult = await _userManager.UpdateAsync(user);
            if (!savePasswordResult.Succeeded)
            {
                // TODO: Log
                _logger.LogError("Error updating Last Passoword Change Date.");
            }

            _logger.LogInformation("User changed their password successfully.");

            StatusMessage = "Your password has been changed.";

            // SJG - Identity Fix :: Redirect to Page Clears TempData - Therefore use Page
            // return RedirectToPage();
            return Page();
        }
    }
}