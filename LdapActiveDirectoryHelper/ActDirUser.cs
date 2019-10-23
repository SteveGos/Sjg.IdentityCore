using System;
using System.ComponentModel;

namespace LdapActiveDirectoryHelper
{
    /// <summary>
    /// LDAP Properties.  Properties that are pulled off of the LDAP record.
    /// </summary>
    [Serializable]
    public class ActDirUser
    {
        ///// <summary>
        ///// From AD: co
        ///// </summary>
        //[DisplayName(@"Country")]
        //public string Country { get; set; } // From AD: co

        ///// <summary>
        ///// From AD: company
        ///// </summary>
        //[DisplayName(@"Company")]
        //public string Company { get; set; } // From AD: company

        ///// <summary>
        ///// From AD: CommonName
        ///// </summary>
        //[DisplayName(@"Common Name")]
        //public string CommonName { get; set; } // From AD: CommonName

        ///// <summary>
        ///// From AD: countryCode
        ///// </summary>
        //[DisplayName(@"Country Code")]
        //public string CountryCode { get; set; } // From AD: countryCode

        ///// <summary>
        ///// From AD: department
        ///// </summary>
        //[DisplayName(@"Department")]
        //public string Department { get; set; } // From AD: department

        ///// <summary>
        ///// From AD: departmentNumber
        ///// </summary>
        //[DisplayName(@"Department Number")]
        //public string UnitCode { get; set; } // From AD: departmentNumber

        ///// <summary>
        ///// From AD: description
        ///// </summary>
        //[DisplayName(@"Description")]
        //public string Description { get; set; } // From AD: description

        /// <summary>
        /// From AD: displayName
        /// </summary>
        [DisplayName(@"DisplayName")]
        public string DisplayName { get; set; } // From AD: displayName

        /// <summary>
        /// From AD: distinguishedName
        /// </summary>
        [DisplayName(@"Distinguished Name")]
        public string DistinguishedName { get; set; } // From AD: distinguishedName

        ///// <summary>
        ///// From AD: employeeNumber
        ///// </summary>
        //[DisplayName(@"Employee Number")]
        //public string EmployeeNumber { get; set; } // From AD: employeeNumber

        ///// <summary>
        ///// From AD: employeeId
        ///// </summary>
        //[DisplayName(@"Employee Id")]
        //public string EmployeeId { get; set; } // From AD: employeeId

        /// <summary>
        /// From AD: givenName
        /// </summary>
        [DisplayName(@"First Name")]
        public string FirstName { get; set; } // From AD: givenName

        ///// <summary>
        ///// From AD: homeDirectory
        ///// </summary>
        //[DisplayName(@"Home Directory")]
        //public string HomeDirectory { get; set; } // From AD: homeDirectory

        ///// <summary>
        ///// From AD: homeDrive
        ///// </summary>
        //[DisplayName(@"Home Drive")]
        //public string HomeDrive { get; set; } // From AD: homeDrive

        ///// <summary>
        ///// From AD: ipPhone
        ///// </summary>
        //[DisplayName(@"IP Phone")]
        //public string IpPhone { get; set; } // From AD: ipPhone

        ///// <summary>
        ///// From AD: l
        ///// </summary>
        //[DisplayName(@"City")]
        //public string City { get; set; } // From AD: l

        ///// <summary>
        ///// From AD: logonCount
        ///// </summary>
        //[DisplayName(@"Logon Count")]
        //public string LogonCount { get; set; } // From AD: logonCount

        /// <summary>
        /// From AD: mail
        /// </summary>
        [DisplayName(@"Email")]
        public string EmailAddress { get; set; } // From AD: mail

        ///// <summary>
        ///// From AD: mailNickname
        ///// </summary>
        //[DisplayName(@"Mail Nickname")]
        //public string MailNickname { get; set; } // From AD: mailNickname

        /////// <summary>
        /////// From AD: manager
        /////// </summary>
        //[DisplayName(@"Manager")]
        //public string ManagerDistinguishedName { get; set; } // From AD: manager

        /////// <summary>
        /////// From AD: Manager - displayName
        /////// </summary>
        //[DisplayName(@"Manager Name")]
        //public string ManagerName { get; set; } // From AD: Manager - displayName

        /////// <summary>
        /////// From AD: Manager - sAMAccountName
        /////// </summary>
        //[DisplayName(@"Manager Login Name")]
        //public string ManagerLoginName { get; set; } // From AD: Manager - sAMAccountName

        /////// <summary>
        /////// From AD: Manager - title
        /////// </summary>
        //[DisplayName(@"Manager Title")]
        //public string ManagerTitle { get; set; } // From AD: Manager - title

        ///// <summary>
        ///// From AD: Manager - employeeNumber
        ///// </summary>
        //[DisplayName(@"Manager Employee Number")]
        //public string ManagerEmployeeNumber { get; set; } // From AD: Manager - employeeNumber

        ///// <summary>
        ///// From AD: msExchHomeServerName
        ///// </summary>
        //[DisplayName(@"Exchange Home Server")]
        //public string ExchangeHomeServer { get; set; } // From AD: msExchHomeServerName

        /// <summary>
        /// From AD: name
        /// </summary>
        [DisplayName(@"Name")]
        public string Name { get; set; } // From AD: name

        /// <summary>
        /// From AD: objectCategory
        /// </summary>
        [DisplayName(@"Object Category")]
        public string ObjectCategory { get; set; } // From AD: objectCategory

        ///// <summary>
        ///// From AD: physicalDeliveryOfficeName
        ///// </summary>
        //[DisplayName(@"Office Name")]
        //public string OfficeName { get; set; } // From AD: physicalDeliveryOfficeName

        ///// <summary>
        ///// From AD: postalCode
        ///// </summary>
        //[DisplayName(@"Postal Code")]
        //public string PostalCode { get; set; } // From AD: postalCode

        ///// <summary>
        ///// From AD: postOfficeBox
        ///// </summary>
        //[DisplayName(@"Post Office Box")]
        //public string PostOfficeBox { get; set; } // From AD: postOfficeBox

        ///// <summary>
        ///// From AD: pwdLastSet
        ///// </summary>
        //[DisplayName(@"Last Password Reset Date")]
        //public DateTime? LastPasswordResetDate { get; set; } // From AD: pwdLastSet

        /// <summary>
        /// From AD: sAMAccountName
        /// </summary>
        [DisplayName(@"Login Name")]
        public string LoginName { get; set; } // From AD: sAMAccountName

        /// <summary>
        /// From AD: sn
        /// </summary>
        [DisplayName(@"Last Name")]
        public string LastName { get; set; } // From AD: sn

        ///// <summary>
        ///// From AD: st
        ///// </summary>
        //[DisplayName(@"State")]
        //public string State { get; set; } // From AD: st

        ///// <summary>
        ///// From AD: streetAddress
        ///// </summary>
        //[DisplayName(@"Street Address")]
        //public string StreetAddress { get; set; } // From AD: streetAddress

        ///// <summary>
        ///// From AD: telephoneNumber
        ///// </summary>
        [DisplayName(@"Telephone Number")]
        public string TelephoneNumber { get; set; } // From AD: telephoneNumber

        ///// <summary>
        ///// From AD: mobile
        ///// </summary>
        //[DisplayName(@"Mobile")]
        //public string Mobile { get; set; } // From AD: mobile

        ///// <summary>
        ///// From AD: title
        ///// </summary>
        [DisplayName(@"Title")]
        public string Title { get; set; } // From AD: title

        ///// <summary>
        ///// From AD: userDirectoryEntry.Path
        ///// </summary>
        //[DisplayName(@"ADS Path")]
        //public string AdsPath { get; set; } // From AD: userDirectoryEntry.Path

        ///// <summary>
        ///// From AD: userPrincipalName
        ///// </summary>
        //[DisplayName(@"User Principle Name")]
        //public string UserPrincipleName { get; set; } // From AD: userPrincipalName

        ///// <summary>
        ///// From AD: whenChanged
        ///// </summary>
        //[DisplayName(@"When Changed")]
        //public DateTime? WhenChanged { get; set; } // From AD: whenChanged

        ///// <summary>
        ///// From AD: whenCreated
        ///// </summary>
        //[DisplayName(@"When Created")]
        //public DateTime? WhenCreated { get; set; } // From AD: whenCreated

        ///// <summary>
        ///// From AD: thumbnailPhoto
        ///// </summary>
        //[DisplayName(@"Photo")]
        //public byte[] ThumbnailPhoto { get; set; } // From AD: thumbnailPhoto

        ///// <summary>
        ///// From AD: thumbnailPhoto - System.Drawing.Imaging.ImageFormat values ie... bmp, emf, exif, gif, guid, icon
        ///// </summary>
        //[DisplayName(@"Image Format")]
        //public string ThumbnailPhotoFormat { get; set; } // From AD: based on thumbnailPhoto

        ///// <summary>
        ///// From AD: facsimileTelephoneNumber
        ///// </summary>
        //[DisplayName(@"Fax")]
        //public string Fax { get; set; } // From AD: facsimileTelephoneNumber

        ///// <summary>
        ///// From AD: thumbnailPhoto
        ///// </summary>
        //[DisplayName(@"Photo")]
        //public byte[] ManagerThumbnailPhoto { get; set; } // From AD: thumbnailPhoto

        ///// <summary>
        ///// From AD: thumbnailPhoto - System.Drawing.Imaging.ImageFormat values ie... bmp, emf, exif, gif, guid, icon
        ///// </summary>
        //[DisplayName(@"Image Format")]
        //public string ManagerThumbnailPhotoFormat { get; set; } // From AD: based on thumbnailPhoto

        /// <summary>
        /// Determined not to be a user Account
        /// </summary>
        public bool IsNonUserAccount { get; set; }

        public int? UserAccountControlFlags { get; set; }

        public bool? IsNormalAccount { get; set; }

        public bool? IsAccountDisabled { get; set; }

        public bool? IsInterDomainTrustAccount { get; set; } // Should Never Be Set for a User Account
        public bool? IsWorkStationTrustAccount { get; set; } // Should Never Be Set for a User Account
        public bool? IsServerTrustAccount { get; set; } // Should Never Be Set for a User Account
        public bool? IsMnsLogonAccount { get; set; } // Should Never Be Set for a User Account
        public bool? IsPartialSecrectsAccount { get; set; } // Should Never Be Set for a User Account

        ////////[DisplayFormat(DataFormatString = "{0:%d}d {0:%h}h {0:%m}m {0:%s}s", ApplyFormatInEditMode = true)]
        ////////public TimeSpan PasswordRemaingTime { get; set; }
    }
}