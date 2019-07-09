using Microsoft.AspNetCore.Identity;
using System;

namespace Sjg.IdentityCore.Utilities
{
    internal class AccAuthLogger
    {
        public static void LogIdentityResult(string userEmail, IdentityResult identityResult, string action)
        {
        }

        public static void LogIdentityResult(Guid userId, IdentityResult identityResult, string action)
        {
        }

        public static void LogActivity(string userEmail, string activity)
        {
        }

        public static void LogActivity(Guid userId, string activity)
        {
        }
    }
}