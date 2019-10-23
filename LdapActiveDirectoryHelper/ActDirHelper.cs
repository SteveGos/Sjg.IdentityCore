using System;
using System.DirectoryServices;

namespace LdapActiveDirectoryHelper
{
    internal static class ActDirHelper
    {
        public static String GetStringProperty(DirectoryEntry userDetail, String propertyName)
        {
            //Debug: Show all Properties
            //var propInfo = string.Empty;
            //foreach (PropertyValueCollection property in userDetail.Properties)
            //{
            //    foreach (object o in property)
            //    {
            //        string value = o.ToString();
            //        propInfo += property.PropertyName + ":" + value + "#";
            //    }
            //}
            //throw new Exception(propInfo);

            return userDetail.Properties.Contains(propertyName) ?
                userDetail.Properties[propertyName][0].ToString().Trim() : string.Empty;
        }

        public static DateTime? GetDateProperty(DirectoryEntry userDetail, String propertyName)
        {
            var str = GetStringProperty(userDetail, propertyName);

            if (string.IsNullOrWhiteSpace(str)) { return null; }

            DateTime date;

            return DateTime.TryParse(str, out date) ? (DateTime?)date : null;
        }

        public static byte[] GetByteArrayProperty(DirectoryEntry userDetail, String propertyName)
        {
            if (!userDetail.Properties.Contains(propertyName)
                || !(userDetail.Properties[propertyName].Value is byte[]))
            {
                return null;
            }

            return (byte[])userDetail.Properties[propertyName].Value;
        }

        ///////// <summary>
        ///////// Check if a User Contol Flag is Set
        ///////// </summary>
        ///////// <param name="directoryentry">Directory Entry</param>
        ///////// <param name="flagToCheck">Flag to Check</param>
        ///////// <returns>returns null if unable to determine.</returns>
        //////public static bool? IsUserAccountControlFlagSet(DirectoryEntry directoryentry, UserFlags flagToCheck)
        //////{
        //////    const string uac = "userAccountControl";
        //////    //if (user.NativeGuid == null) return false;
        //////    if (directoryentry.NativeGuid == null) return null;

        //////    if (directoryentry.Properties[uac] != null && directoryentry.Properties[uac].Value != null)
        //////    {
        //////        var userFlags = (UserFlags)directoryentry.Properties[uac].Value;
        //////        return userFlags.Contains(flagToCheck);
        //////    }

        //////    //return false;
        //////    return null;
        //////}

        /// <summary>
        /// Check if a User Contol Flag is Set
        /// </summary>
        /// <param name="actDirUser">ActDi</param>
        /// <param name="flagToCheck">Flag to Check</param>
        /// <returns>returns null if unable to determine.</returns>
        public static bool? IsUserAccountControlFlagSet(ActDirUser actDirUser, UserFlags flagToCheck)
        {
            if (actDirUser?.UserAccountControlFlags == null)
            {
                return null;
            }

            return ((UserFlags)actDirUser.UserAccountControlFlags & flagToCheck) == flagToCheck;
        }

        //public static bool GetBoolProperty(DirectoryEntry userDetail, String propertyName)
        //{
        //    if (userDetail.Properties.Contains(propertyName))
        //    {
        //        var s = userDetail.Properties[propertyName][0].ToString().Trim();
        //        //return (bool)userDetail.Properties["msDS-UserAccountDisabled"].Value;
        //        return (bool)userDetail.Properties["propertyName"].Value;
        //    }

        //    return true;
        //}
    }
}