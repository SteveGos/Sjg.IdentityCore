
# Sjg.IdentityCore
Identity Framework - Extended/Enhanced

This is a razor class library that extends / enhances `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.  

It adds on the ability to 

- manage users, 
- allow the site to self register or register by invitation only.

# Sample Web Site
**Project :** SampleWebSite is an Asp.Net Core 2.2 Razor Pages web application that uses Sjg.IdentityCore for the identity framework user management.

The web site was initially created in VS 2019 by creating a new Web Application based on Razor Pages.  No Authentication and configure for HTTPS.  Docker was not used.

# Integrating into a Razor Pages Web Application.
These notes will use the SampleWebSite for reference.

## Hosting Start Up
Hosting Startup is implemented to provide the configuration providers or services for Sjg.IdentityCore.

See `IdentityHostingStartup.cs` to see notes and requirements for a web application.

## Implementation Notes
See the following modules/files to assist in implementation.

- IdentityHostingStartup.cs
- Startup.cs
- appsettings.json 

Configuration 

# Self Registration - Initial User Setup
Use the website to self register.

# Register By Invitation Only - Initial User Setup
If website is by invitation only, Manually create an invitation, and use navigate to invitation link to complete registration.

```SQL
Declare @email nvarchar (256)
Declare @code nvarchar (256)

SET @email = 'a1@a.com'  -- Email Address example: abc@def.com
SET @code = 'SETUPCODE123'  --  Some SetUp Code to be accessed right away

-- https://{website}/Identity/Account/Register?inviteCode=SETUPCODE123  --  Use SetUp Code From Above

INSERT INTO [Sjg.IdentityCore].[AccAuthInvites] ([AccAuthInviteId],[Email],[ExpirationDateUtc],[DisplayName],[Code],[IsServiceAccount], [PasswordNeverExpires])
     VALUES
           ( NewId()         --<AccAuthInviteId, uniqueidentifier,> 
		     ,@email         --<Email, nvarchar(256),>
             ,Getdate() + 1  --<ExpirationDateUtc, datetime2(7),>
             ,@email         --<DisplayName, nvarchar(100),>
             ,@code          -- <Code, nvarchar(128),>
             ,0              -- <IsServiceAccount, bit,>
			 ,0              -- ,<PasswordNeverExpires, bit,>
			)

Select 'https://{website}/Identity/Account/Register?inviteCode=' + @code as NavigateLink
```

# Assigning an initial Access Administrator

Assign User Administration authorization by putting the user in the `Identity - User Administrator` role using the SQL below.

```SQL
Declare @username nvarchar (256)
Declare @rolename nvarchar (256)

SET @username = 'useradminemail@domain.com'  -- Email Address or windows/active Directory example: abc@def.com or domain\jsmith
SET @rolename = 'Identity - User Administrator'

INSERT INTO [Sjg.IdentityCore].[AspNetUserRoles] ([UserId], [RoleId])
VALUES
  ((SELECT Top 1 [Id] FROM [Sjg.IdentityCore].[AspNetUsers] Where [UserName] = @username),
   (SELECT Top 1 [Id] FROM [Sjg.IdentityCore].[AspNetRoles] Where [Name]     = @rolename))
```
