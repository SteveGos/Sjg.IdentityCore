using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTest
{
    public class ActDirValidationApi : Sjg.IdentityCore.ActiveDirectory.IActiveDirApi
    {
        public string GetLoginNameFromEmail(string ldapDomain, string ldapDomainUsername, string ldapDomainPassword, string email)
        {
            throw new NotImplementedException();
        }

        public bool ValidateCredentials(string ldapDomain, string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
