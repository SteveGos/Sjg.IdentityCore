using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace LdapActiveDirectoryHelper
{
    internal class DistinctPrincipalComparer : IEqualityComparer<Principal>
    {
        public bool Equals(Principal x, Principal y)
        {
            return x.DistinguishedName.Equals(y.DistinguishedName, System.StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Principal obj)
        {
            return obj.DistinguishedName.GetHashCode();
        }
    }
}