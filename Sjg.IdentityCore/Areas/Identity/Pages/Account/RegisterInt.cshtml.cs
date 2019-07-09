using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;
using Sjg.IdentityCore.Utilities.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterIntModel : PageModel
    {
        private readonly SignInManager<AccAuthUser> _signInManager;
        private readonly UserManager<AccAuthUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IAccAuthConfiguration _accAuthConfiguration;
        private readonly IAccAuthEmailSender _accAuthEmailSender;
        private readonly IAccAuthViewRenderService _accAuthViewRenderService;
        private readonly AccAuthContext _accAuthContext;

        public RegisterIntModel(
            UserManager<AccAuthUser> userManager,
            SignInManager<AccAuthUser> signInManager,
            ILogger<RegisterModel> logger,
            IAccAuthConfiguration accAuthConfiguration,
            IAccAuthViewRenderService accAuthViewRenderService,
            IAccAuthEmailSender accAuthEmailSender,
            AccAuthContext accAuthContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accAuthConfiguration = accAuthConfiguration;
            _accAuthViewRenderService = accAuthViewRenderService;
            _accAuthEmailSender = accAuthEmailSender;
            _accAuthContext = accAuthContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "InviteCode")]
            public string InviteCode { get; set; }
        }

        // public void OnGet(string inviteCode, string returnUrl = null)
        public IActionResult OnGet(string inviteCode, string returnUrl = null)
        {
            if (!_accAuthConfiguration.AllowLdap)
            {
                return RedirectToPage("./Register", new { Input.InviteCode, returnUrl });
            }

            if (!string.IsNullOrWhiteSpace(inviteCode))
            {
                var invite = _accAuthContext.AccAuthInvites.FirstOrDefault(o => o.Code == inviteCode);
                if (invite != null)
                {
                    Input = new InputModel { Email = invite.Email, InviteCode = inviteCode };
                }
                else
                {
                    Input = new InputModel { InviteCode = inviteCode };
                }
            }

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccAuthInvite invite = null;

                    if (_accAuthConfiguration.InvitationOnly)
                    {
                        if (string.IsNullOrWhiteSpace(Input.InviteCode))
                        {
                            ModelState.AddModelError(string.Empty, "Invitation code required.");
                            return Page();
                        }

                        invite = _accAuthContext.AccAuthInvites
                            .Include(o => o.AccAuthInviteRoles)
                            .FirstOrDefault(o => o.Code == Input.InviteCode && o.Email == Input.Email);

                        if (invite == null)
                        {
                            ModelState.AddModelError(string.Empty, "Valid Invitations Information required.");
                            return Page();
                        }
                        if (invite.ExpirationDateUtc < DateTime.UtcNow)
                        {
                            ModelState.AddModelError(string.Empty, "Your invitation has expired, please notify application support.");
                            return Page();
                        }
                    }

                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    if (user != null)
                    {
                        if (invite != null)
                        {
                            _accAuthContext.AccAuthInvites.Remove(invite);
                            _accAuthContext.SaveChanges();
                        }

                        return RedirectToPage("./Login");
                    }

                    user = new AccAuthUser { UserName = Input.Email, Email = Input.Email };

                    user.IsActive = true;
                    user.IsActiveDirectoryUser = true;

                    // For LDAP user - Generate Random Password - Which will never be used to authenticate but needed for record.
                    var password = PasswordUtil.GeneratePassword(_signInManager.UserManager.Options);

                    var result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        // Email Sender - Confirm Email

                        var callbackUri = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code },
                            protocol: Request.Scheme);

                        var model = new Views.Email.AccAuth.AccAuthSendVerificationEmailViewModel
                        {
                            CallBackUrl = callbackUri.ToString(),
                            Code = code,
                            Email = Input.Email,
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

                        //await _accAuthEmailSender.SendEmailAsync(
                        //    Input.Email,
                        //    "Confirm your email",
                        //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        //ModelState.AddModelError(string.Empty, sjg);
                        //return Page();

                        var emailComponents = Input.Email.Split('@');
                        if (emailComponents.Length == 2)
                        {
                            user.EmailDomainName = emailComponents[1].ToLower();
                        }
                        else
                        {
                            user.EmailDomainName = null;
                        }

                        var tsk = await _userManager.UpdateAsync(user);

                        var roleList = new List<string>();

                        if (invite != null)
                        {
                            foreach (var r in invite.AccAuthInviteRoles)
                            {
                                var accAuthInviteRoles = await _accAuthContext.AccAuthInviteRoles.Include(o => o.AccessRole)
                                    .Where(o => o.AccAuthInviteId == invite.AccAuthInviteId)
                                    .ToListAsync();

                                foreach (var item in accAuthInviteRoles)
                                {
                                    roleList.Add(r.AccessRole.Name);
                                }
                            }
                        }

                        if (invite != null)
                        {
                            _accAuthContext.AccAuthInvites.Remove(invite);
                            _accAuthContext.SaveChanges();
                        }

                        foreach (var item in roleList)
                        {
                            await _userManager.AddToRoleAsync(user, item);
                        }

                        //var usr = _accAuthContext.Users.Where(o => o.Id == user.Id).FirstOrDefault();
                        //if (usr != null)
                        //{
                        //    var emailComponents = Input.Email.Split('@');
                        //    if (emailComponents.Length == 2)
                        //    {
                        //        usr.EmailDomainName = emailComponents[1].ToLower();
                        //        var tsk = await _accAuthContext.SaveChangesAsync();
                        //    }
                        //}

                        return RedirectToPage("./ConfirmEmailSent");

                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.GetBaseException().Message);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}