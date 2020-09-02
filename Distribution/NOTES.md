# Sjg.IdentityCore
Identity Framework - Extended/Enhanced

This is a razor class library that extends / enhances `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.  

It adds on the ability to 

- manage users, 
- allow the site to self register or register by invitation only.
- Implement LDAP verification - targeted for internal applications.

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

SET @email = 'A1@a.com'  -- Email Address example: abc@def.com
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

# Emails

The Emails that are generated use template.  See the folder Views\Email\AccAuth and read the _ReadMe.md file.

A Copy of the original templates are located in Views\Email\AccAuth_OriginalTemplates.

# ApiBasicAuthorizeAttribute

There is a Basic Authorization Action Filter that can be utilized for APIS.  See Examples in Sample WebSite API Folder.

# .Net Core 3.1+ Web Application

In `IdentityHostingStartup`  add new Static Method `AddIdentity`.   

## IdentityHostingStartup AccAuthConfigure Code

```csharp
private static void AccAuthConfigure(IWebHostBuilder builder)
{
    builder.ConfigureServices((context, services) =>
    {
        // To Use a Custom Authentication Provider - Add Class that implements ICustomUserAuthenticatorProvider
        //services.AddScoped<ICustomUserAuthenticatorProvider, {yourCustomUserAuthenticatorProvider}>();

        var ldapConfigType = typeof(Providers.LdapCustomUserAuthConfig);
        var ldapConfig = context.Configuration.GetSection(ldapConfigType.FullName).Get<Providers.LdapCustomUserAuthConfig>();

        services.TryAddSingleton<Providers.ILdapCustomUserAuthConfig>(ldapConfig);

        services.TryAddScoped<ICustomUserAuthenticatorProvider, Providers.LdapCustomUserAuth>();

        // Sjg.IdentityCore - Set Up - Razor Class Libraries Extensions (Sets up Authentication, Static Files, etc...)

        // .Net Core 2.2 
        // services.AddIdentityCoreExtensions(context.Configuration); // Set Up Sjg.IdentityCore

        // .Net Core 3.1+ (should work for .net Core 2.2 as well)
        AddIdentity(services, context.Configuration);
    });
}

```

## IdentityHostingStartup AddIdentity Code

```csharp
/// <summary>
/// Sets up --- CORE 3.1
/// </summary>
/// <param name="services"></param>
/// <param name="configuration"></param>
public static void AddIdentity(IServiceCollection services, IConfiguration configuration)
{
    // Add Configuration (if needed)
    services.ConfigureOptions(typeof(IdentityCoreConfigureOptions));

    // Add Services for RCL (if needed)

    // Use services.Try* - for avoiding duplicate services...
    // Example:
    // Use services.TryAddSingleton -  checks for the existence of the service descriptor in the
    //                                 service collection and any only adds it if it's not already there

    // AccAuthGridPagerTagHelper - Services Needed -
    //    IHttpContextAccessor, IActionContextAccessor, and IUrlHelperFactory
    // -- Ensure they are added
    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
    services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();

    var accAuthConfig = configuration.GetSection(typeof(AccAuthConfiguration).FullName).Get<AccAuthConfiguration>();

    services.TryAddSingleton<IAccAuthConfiguration>(accAuthConfig);

    // Connection String is the Name Space => Sjg.IdentityCore => typeof(AccAuthContext).Namespace
    services.AddDbContext<AccAuthContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(typeof(AccAuthContext).Namespace),
                x =>
                {
                    // -----------------------------------------------------------
                    // The MigrationsAssembly must have the Assembly Name Space AccAuthContext -
                    //      typeof(AccAuthContext).Namespace
                    // -----------------------------------------------------------
                    x.MigrationsAssembly(typeof(AccAuthContext).Namespace);
                    x.MigrationsHistoryTable(AccAuthContext.MigrationHistoryTable, AccAuthContext.DefaultSchema);
                })
            );

    services.AddIdentity<AccAuthUser, AccAuthRole>(options =>
    {
        options.Password = accAuthConfig.PasswordOptions;
        options.Lockout = accAuthConfig.LockoutOptions;
        options.SignIn = accAuthConfig.SignInOptions;
        options.User = accAuthConfig.UserOptions;
    })

        .AddEntityFrameworkStores<AccAuthContext>()
        .AddDefaultTokenProviders()
        // .AddDefaultUI() // SJG ????

        .AddUserManager<UserManager<AccAuthUser>>()
        .AddRoleManager<RoleManager<AccAuthRole>>()

        //.AddUserStore<UserStore<AccAuthUser, AccAuthRole, AccAuthContext, Guid>>()
        .AddUserStore<AccAuthUserStore>()

        //.AddRoleStore<RoleStore<AccAuthRole, AccAuthContext, Guid>>()
        .AddRoleStore<AccAuthRoleStore>();

    services.TryAddSingleton<IAccAuthEmailSender, AccAuthEmailSender>();
    services.TryAddScoped<IAccAuthViewRenderService, AccAuthViewRenderService>();

    //// AddAuthorization Must Be Before AddMvc
    //  Add a policy for a User Administrator...
    //     It will require role Sjg.IdentityCore.Areas.UserMgmt.Roles.UserAdministrator

    var policyName = "Areas.UserMgmt.Roles.UserAdministrator";
    services.AddAuthorization(options =>
    {
        options.AddPolicy(policyName,
            policy => policy.RequireRole(Sjg.IdentityCore.Areas.UserMgmt.AccAuthRoles.RoleNameUserAdministrator));
    });

    services.AddMvc().AddRazorPagesOptions(options =>
    {
        // Area -- Identity - Set Authorization.
        //options.Conventions.AllowAnonymousToAreaFolder("Identity", "/");
        options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");

        // Area -- UserMgmt => based on Policy
        options.Conventions.AuthorizeAreaFolder("UserMgmt", "/", policyName); // All of Area
    });

    // Sjg.IdentityCore - Establish Access/Authorization Roles for User Management
    //Task.Run(() => Sjg.IdentityCore.Areas.UserMgmt.Roles.SetUpRolesAsync(app.ApplicationServices));  // Process Asynchronously - no need to wait.

    //Areas.UserMgmt.Roles.SetUpRolesAsync(services);  // Process Asynchronously - no need to wait.

    // Sjg.IdentityCore - Establish Access/Authorization Roles for User Management
    using (var serviceProvider = services.BuildServiceProvider())
    {
        // Process Asynchronously - no need to wait.
        _ = AccAuthRoleDefinitions.SetUpRolesAsync<Sjg.IdentityCore.Areas.UserMgmt.AccAuthRoles>(serviceProvider);
    }
}
```