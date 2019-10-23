using System.DirectoryServices;

namespace LdapActiveDirectoryHelper
{
    internal class Hydrate
    {
        //public static ActDirUser HydrateAdUser(DirectoryEntry userDirectoryEntry, bool includeImage)
        //{
        //    return HydrateActiveDirectoryUser(userDirectoryEntry, includeImage);
        //}

        public static ActDirUser HydrateAdUser(DirectoryEntry userDirectoryEntry)
        {
            return HydrateActiveDirectoryUser(userDirectoryEntry, false);
        }

        private static ActDirUser HydrateActiveDirectoryUser(DirectoryEntry userDirectoryEntry, bool includeImage = false)
        {
            if (userDirectoryEntry == null)
            {
                return null;
            }

            var user = new ActDirUser
            {
                //City = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.City),
                //CommonName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.CommonName),
                //Company = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Company),
                //Country = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Country),
                //CountryCode = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.CountryCode),
                //Department = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Department),
                //Description = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Description),
                DisplayName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.DisplayName),
                DistinguishedName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.DistinguishedName),
                EmailAddress = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.EmailAddress),
                //EmployeeNumber = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.EmployeeNumber),
                //EmployeeId = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.EmployeeId),
                //ExchangeHomeServer = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.ExchangeHomeServer),
                FirstName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.FirstName),
                //HomeDirectory = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.HomeDirectory),
                //HomeDrive = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.HomeDrive),
                //IpPhone = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.IpPhone),
                LastName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.LastName),
                //LastPasswordResetDate = ActDirHelper.GetDateProperty(userDirectoryEntry, ActDirProperties.LastPasswordResetDate),
                LoginName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.LoginName),
                //LogonCount = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.LogonCount),
                //MailNickname = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.MailNickname),
                //ManagerDistinguishedName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.ManagerDistinguishedName),
                //Mobile = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Mobile),
                Name = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Name),
                ObjectCategory = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.ObjectCategory),
                //OfficeName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.OfficeName),
                //PostalCode = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.PostalCode),
                //PostOfficeBox = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.PostOfficeBox),
                //State = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.State),
                //StreetAddress = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.StreetAddress),
                TelephoneNumber = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.TelephoneNumber),
                Title = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Title),
                //UnitCode = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.UnitCode),
                //UserPrincipleName = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.UserPrincipleName),
                //WhenChanged = ActDirHelper.GetDateProperty(userDirectoryEntry, ActDirProperties.WhenChanged),
                //WhenCreated = ActDirHelper.GetDateProperty(userDirectoryEntry, ActDirProperties.WhenCreated),

                //ManagerName = string.Empty,
                //ManagerLoginName = string.Empty,
                //ManagerTitle = string.Empty,
                //ManagerEmployeeNumber = string.Empty,

                //ThumbnailPhoto = includeImage
                //                    ? ActDirHelper.GetByteArrayProperty(userDirectoryEntry, ActDirProperties.ThumbnailPhoto)
                //                    : null,

                //Fax = ActDirHelper.GetStringProperty(userDirectoryEntry, ActDirProperties.Fax),
                UserAccountControlFlags = null,

                IsNormalAccount = null,
                IsAccountDisabled = null,

                IsInterDomainTrustAccount = null, // Should Never Be Set for a User Account
                IsWorkStationTrustAccount = null, // Should Never Be Set for a User Account
                IsServerTrustAccount = null, // Should Never Be Set for a User Account
                IsMnsLogonAccount = null, // Should Never Be Set for a User Account
            };

            if (userDirectoryEntry.NativeGuid != null)
            {
                if (userDirectoryEntry.Properties["userAccountControl"] != null && userDirectoryEntry.Properties["userAccountControl"].Value != null)
                {
                    user.UserAccountControlFlags = (int)userDirectoryEntry.Properties["userAccountControl"].Value;
                }
            };

            user.IsNormalAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.NormalAccount);
            user.IsAccountDisabled = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.AccountDisabled);

            // Should Never Be Set for a User Account
            user.IsInterDomainTrustAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.InterDomainTrustAccount);
            user.IsWorkStationTrustAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.WorkstationTrustAccount);
            user.IsServerTrustAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.ServerTrustAccount);
            user.IsMnsLogonAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.MnsLogonAccount);
            user.IsPartialSecrectsAccount = ActDirHelper.IsUserAccountControlFlagSet(user, UserFlags.PartialSecretsAccount);

            if (user.UserAccountControlFlags == null)
            {
                user.IsNonUserAccount = true;
            }
            else
            {
                user.IsNonUserAccount =
                              user.IsInterDomainTrustAccount.HasValue ? user.IsInterDomainTrustAccount.Value : true
                           || user.IsWorkStationTrustAccount.HasValue ? user.IsWorkStationTrustAccount.Value : true
                           || user.IsServerTrustAccount.HasValue ? user.IsServerTrustAccount.Value : true
                           || user.IsMnsLogonAccount.HasValue ? user.IsMnsLogonAccount.Value : true
                           || user.IsPartialSecrectsAccount.HasValue ? user.IsPartialSecrectsAccount.Value : true;
            }

            ////////// MaxPasswordAge and PasswordAge is null --- May not be available
            ////////if (userDirectoryEntry.Properties["MaxPasswordAge"] != null && userDirectoryEntry.Properties["MaxPasswordAge"].Value != null &&
            ////////    userDirectoryEntry.Properties["PasswordAge"] != null && userDirectoryEntry.Properties["PasswordAge"].Value != null)
            ////////{
            ////////    var maxPasswordAge = (int)userDirectoryEntry.Properties["MaxPasswordAge"].Value;
            ////////    var passwordAge = (int)userDirectoryEntry.Properties["PasswordAge"].Value;
            ////////    user.PasswordRemaingTime = TimeSpan.FromSeconds(maxPasswordAge) - TimeSpan.FromSeconds(passwordAge);
            ////////}

            //////////////////////////if (includeImage && user.ThumbnailPhoto != null)
            //////////////////////////{
            //////////////////////////    using (var imageMemStream = new MemoryStream(user.ThumbnailPhoto))
            //////////////////////////    {
            //////////////////////////        using (var bitmap = new Bitmap(imageMemStream))
            //////////////////////////        {
            //////////////////////////            if (bitmap.RawFormat.Equals(ImageFormat.Jpeg))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Jpeg.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Bmp))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Bmp.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Png))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Png.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Emf))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Emf.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Exif))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Exif.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Gif))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Gif.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Icon))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Icon.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.MemoryBmp))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.MemoryBmp.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Tiff))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Tiff.ToString();
            //////////////////////////            else if (bitmap.RawFormat.Equals(ImageFormat.Wmf))
            //////////////////////////                user.ThumbnailPhotoFormat = ImageFormat.Wmf.ToString();
            //////////////////////////            else
            //////////////////////////                user.ThumbnailPhotoFormat = null;
            //////////////////////////        }
            //////////////////////////    }
            //////////////////////////}

            return user;
        }
    }
}