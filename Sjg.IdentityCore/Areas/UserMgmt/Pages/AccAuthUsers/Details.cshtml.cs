using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sjg.IdentityCore.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthUsers
{
    public class DetailsModel : PageModel
    {
        public const string btnActive = "Change Active";
        public const string btnFrozen = "Change Frozen";
        public const string btnChangeSvcAccount = "Change Service Account";
        public const string btnClearLockout = "Clear Lockout";
        public const string btnRemove = "Remove User";

        //private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<AccAuthUser> _userManager;

        private readonly AccAuthContext _accAuthCtx;

        public DetailsModel(AccAuthContext accAuthContext, UserManager<AccAuthUser> userManager)
        {
            _accAuthCtx = accAuthContext;
            _userManager = userManager;
        }

        public AccAuthUser AccAuthUser { get; set; }

        [Display(Name = "Locked Out")]
        public bool IsLockedOut { get; set; }

        public void OnGet(Guid id)
        {
            AccAuthUser = _accAuthCtx.Users.FirstOrDefault(o => o.Id == id);

            if (AccAuthUser == null)
            {
                ModelState.AddModelError(string.Empty, "User no longer exists.");
                return;
            }

            IsLockedOut = _userManager.IsLockedOutAsync(AccAuthUser).Result;
        }

        public void OnPostAsync(Guid id, string command = null)
        {
            AccAuthUser = _accAuthCtx.Users.FirstOrDefault(o => o.Id == id);

            if (AccAuthUser == null)
            {
                ModelState.AddModelError(string.Empty, $"{DateTime.Now} User no longer exists.");
                return;
            }

            //IsLockedOut = _userManager.IsLockedOutAsync(AccAuthUser).Result;

            int rslt;
            IdentityResult lockoutResult;

            command = command ?? string.Empty;
            switch (command)
            {
                case btnActive:
                    AccAuthUser.IsActive = !AccAuthUser.IsActive;
                    rslt = _accAuthCtx.SaveChangesAsync().Result;

                    break;

                case btnFrozen:
                    AccAuthUser.IsFrozen = !AccAuthUser.IsFrozen;
                    rslt = _accAuthCtx.SaveChangesAsync().Result;

                    break;

                case btnChangeSvcAccount:
                    AccAuthUser.IsServiceAccount = !AccAuthUser.IsServiceAccount;
                    rslt = _accAuthCtx.SaveChangesAsync().Result;
                    break;

                case btnClearLockout:
                    // You can set the DateTimeOffset to anything you want as long as it's before the current DateTimeUTC,
                    //  - Use DateTime.UtcNow as it gives the added benefit of knowing when the account was unlocked.

                    // When the user eventually logs in again the AccessFailedCount will be reset to 0, so you don't need to worry about resetting that.

                    lockoutResult = _userManager.SetLockoutEndDateAsync(AccAuthUser, new DateTimeOffset(DateTime.UtcNow)).Result;

                    if (!lockoutResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, $"{DateTime.Now} Lockout Reset Failed");
                        foreach (var item in lockoutResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, $" - {item.Code} {item.Description}");
                        }
                    }

                    break;

                case btnRemove:

                    var userEmail = AccAuthUser.Email;

                    var deleteResult = _userManager.DeleteAsync(AccAuthUser).Result;

                    if (deleteResult.Succeeded)
                    {
                        AccAuthUser = null;
                        break;
                    }

                    ModelState.AddModelError(string.Empty, $"{DateTime.Now} Removal Failed");
                    foreach (var item in deleteResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, $" - {item.Code} {item.Description}");
                    }
                    break;

                default:
                    ModelState.AddModelError(string.Empty, $"{DateTime.Now} Unknown Action");
                    break;
            }

            // If we got this far, something failed, redisplay form
            //return Page();
        }
    }
}