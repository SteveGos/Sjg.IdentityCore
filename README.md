# Sjg.IdentityCore
Identity Framework - Extended/Enhanced

This is a razor class library that entends / enhances `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.  

It adds on the ability to 

- managge users, 
- allow the site to self register or register by invitation only.


# AppSettings
See `appsettings.json` in project `WebAppTest` for integrating `Sjg.IdentityCore`.

# Assigning an initial user as as an Access Administrator

Self register a user that will be the User Administrator.  Give this user user administration authorizatin by 
putting him in the `Identity - User Administrator` role using the SQL below.

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

# Code First Migrations

To set up the SQL database - use Code First Migrations.

It is assumed that the developer has at least a basic knowledge of Code First Migrations.

### Quick Start

**Note** Change ***WebAppTest*** to your Web Application.

##### Add Migration

Add-Migration -Name Initial -OutputDir Migrations -Context Sjg.IdentityCore.AccAuthContext -Project WebAppTest -StartupProject WebAppTest

SYNTAX
```
    Add-Migration [-Name] <String> [-OutputDir <String>] [-Context <String>] [-Environment <String>] [-Project <String>] [-StartupProject <String>] [<CommonParameters>]
```

##### Remove Migration

Remove-Migration -Context Sjg.IdentityCore.AccAuthContext -Project WebAppTest -StartupProject WebAppTest

SYNTAX
```
    Remove-Migration [-Force] [-Context <String>] [-Environment <String>] [-Project <String>] [-StartupProject <String>] [<CommonParameters>]
```

##### Update Database 

Update-Database -Context Sjg.IdentityCore.AccAuthContext -Project WebAppTest -StartupProject WebAppTest

*Remove All Migrations from database*

Update-Database -Context Sjg.IdentityCore.AccAuthContext -Project WebAppTest -StartupProject WebAppTest -Migration 0

SYNTAX
```
    Update-Database [[-Migration] <String>] [-Context <String>] [-Environment <String>] [-Project <String>] [-StartupProject <String>] [<CommonParameters>]
```

##### Script Database

Script-Migration -Idempotent -Context Sjg.IdentityCore.AccAuthContext -Project WebAppTest -StartupProject WebAppTest 

# Integrating into a Web Application - Razor Pages

See `IdentityHostingStartup.cs` in project `WebAppTest` for integrating `Sjg.IdentityCore`.

The Roles for User administration must be set up.  This can be done in the startup as seen below and 
in `Startup.cs` in project `WebAppTest`.

```csharp
// Sjg.IdentityCore - Establish Access/Authorization Roles for User Managament
Task.Run(() => Sjg.IdentityCore.Areas.UserMgmt.Roles.SetUpRolesAsync(app.ApplicationServices));  // Process Asynchronously - no need to wait.
```