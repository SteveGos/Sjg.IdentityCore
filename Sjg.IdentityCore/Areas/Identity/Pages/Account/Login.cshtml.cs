using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sjg.IdentityCore.ActiveDirectory;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [Attributes.ViewLayout("_Layout_Login")]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AccAuthUser> _signInManager;

        private readonly ILogger<LoginModel> _logger;
        private readonly IAccAuthConfiguration _accAuthConfiguration;
        private readonly IActiveDirApi _activeDirApi;

        public LoginModel(
            SignInManager<AccAuthUser> signInManager,
            UserManager<AccAuthUser> userManager,
            ILogger<LoginModel> logger,
            IAccAuthConfiguration accAuthConfiguration,
            IActiveDirApi activeDirApi)
        {
            _signInManager = signInManager;
            _logger = logger;
            _accAuthConfiguration = accAuthConfiguration;
            _activeDirApi = activeDirApi;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ModelState.Clear();

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            IdentityResult identityResult;

            if (ModelState.IsValid)
            {
                var accessUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                if (accessUser != null)
                {
                    if (!accessUser.IsActive)
                    {
                        _logger.LogWarning($"{Input.Email} User account locked out.");
                        return RedirectToPage("./InActive");
                    }

                    if (_signInManager.Options.SignIn.RequireConfirmedEmail && !accessUser.EmailConfirmed)
                    {
                        _logger.LogWarning($"{Input.Email} Email Confirmation required");
                        return RedirectToPage("./ConfirmEmailRequired");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect user ID or password. Type the correct user ID and password, and try again. (A)");
                    return Page();
                }

                // See if LDAP
                if (_accAuthConfiguration.AllowLdap &&
                    !string.IsNullOrWhiteSpace(_accAuthConfiguration.LdapDomain) &&
                    !string.IsNullOrWhiteSpace(_accAuthConfiguration.LdapEmail))
                {
                    if (Input.Email.EndsWith(_accAuthConfiguration.LdapEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        // Force Two Factor Off for AD Users
                        if (accessUser != null && accessUser.TwoFactorEnabled)
                        {
                            accessUser.TwoFactorEnabled = false;
                            identityResult = await _signInManager.UserManager.UpdateAsync(accessUser);
                            if (!identityResult.Succeeded)
                            {
                                Utilities.AccAuthLogger.LogIdentityResult(Input.Email, identityResult, "AD User - Turn Off Two Factor Authentication");
                            }
                        }

                        // Get User name From LDAP using Email...
                        var loginName = _activeDirApi.GetLoginNameFromEmail(_accAuthConfiguration.LdapDomain,
                            _accAuthConfiguration.LdapUsername,
                            _accAuthConfiguration.LdapPassword,
                            Input.Email);

                        var validated = false;
                        if (!string.IsNullOrWhiteSpace(loginName))
                        {
                            validated = _activeDirApi.ValidateCredentials(
                                _accAuthConfiguration.LdapDomain, loginName, Input.Password);
                        }

                        if (!validated)
                        {
                            ModelState.AddModelError(string.Empty, "Incorrect user ID or password. Type the correct user ID and password, and try again. (B)");
                            return Page();
                        }

                        // For Security Purposes, Every time an AD user signs on
                        //   - regenerate the password in Identity Tables
                        identityResult = await _signInManager.UserManager.RemovePasswordAsync(accessUser);

                        if (!identityResult.Succeeded)
                        {
                            Utilities.AccAuthLogger.LogIdentityResult(Input.Email, identityResult, "AD User - Remove Application Password");
                        }
                        else
                        {
                            var password = Utilities.PasswordUtil.GeneratePassword(_signInManager.UserManager.Options);
                            identityResult = await _signInManager.UserManager.AddPasswordAsync(accessUser, password);

                            if (!identityResult.Succeeded)
                            {
                                Utilities.AccAuthLogger.LogIdentityResult(Input.Email, identityResult, "AD User - Random Application Password");
                            }

                            await _signInManager.SignInAsync(accessUser, true);

                            var emailComponents = accessUser.Email.Split('@');
                            if (emailComponents.Length == 2)
                            {
                                accessUser.EmailDomainName = emailComponents[1].ToLower();
                            }

                            accessUser.LastLoginDateTimeUtc = DateTime.UtcNow;
                            accessUser.IsActiveDirectoryUser = true;
                            identityResult = await _signInManager.UserManager.UpdateAsync(accessUser);

                            if (!identityResult.Succeeded)
                            {
                                Utilities.AccAuthLogger.LogIdentityResult(Input.Email, identityResult, "AD User - Update Last Login");
                            }

                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        if (_accAuthConfiguration.OnlyLdap)
                        {
                            ModelState.AddModelError(string.Empty, "Incorrect user ID or password. Type the correct user ID and password, and try again. (C)");
                            return Page();
                        }
                    }
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"{Input.Email} User logged in.");

                    // Re-retrieve access user in case PasswordSignInAsync updated record
                    var usr = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                    if (usr != null)
                    {
                        // Update Email Domain and Last Login Async.. Do Not Wait....
                        var emailComponents = usr.Email.Split('@');
                        if (emailComponents.Length == 2)
                        {
                            usr.EmailDomainName = emailComponents[1].ToLower();
                        }

                        // Update
                        usr.LastLoginDateTimeUtc = System.DateTime.UtcNow;
                        var updTask = await _signInManager.UserManager.UpdateAsync(usr);
                    }

                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning($"{Input.Email} User account locked out.");
                    return RedirectToPage("./Lockout");
                }

                if (result.IsNotAllowed)
                {
                    // Re-retrieve access user in case PasswordSignInAsync updated record
                    accessUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                    if (accessUser != null)
                    {
                        if (_signInManager.Options.SignIn.RequireConfirmedEmail && !accessUser.EmailConfirmed)
                        {
                            _logger.LogWarning($"{Input.Email} Email Confirmation required");
                            return RedirectToPage("./ConfirmEmailRequired");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt (C).");
                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}