# Sjg.IdentityCore
Identity Framework - Extended/Enhanced

This is a razor class library that extends / enhances `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.  

It adds on the ability to 

- manage users, 
- allow the site to self register / register by invitation only.
- Implement LDAP verification - targeted for internal applications.

# Web Application Integration
Web application integration is done via a NUGET package.  The NUGET packages are available in the Distribution folder.

# Security and Identity

[Overview of ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-2.2)

[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=visual-studio)

[Introduction to authorization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-2.2)

[ASP.NET Core Data Protection](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/introduction?view=aspnetcore-2.2)

[Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows)

# Configuring Windows Authentication

[Configure Windows Authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-2.2&tabs=visual-studio)

Windows Authentication isn't supported with HTTP/2. Authentication challenges can be sent on HTTP/2 responses, but the client must downgrade to HTTP/1.1 before authenticating.

# Web APIs

[Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2)


# Code First Migrations

To set up the SQL database - use Code First Migrations.

It is assumed that the developer has at least a basic knowledge of Code First Migrations.

# Integrating into a Web Application - Razor Pages

See `IdentityHostingStartup.cs` in project `WebAppTest` for integrating `Sjg.IdentityCore`.

The Roles for User administration must be set up.  This can be done in the startup as seen below and 
in `Startup.cs` in project `WebAppTest`.

```csharp
// Sjg.IdentityCore - Establish Access/Authorization Roles for User Management
Task.Run(() => Sjg.IdentityCore.Areas.UserMgmt.Roles.SetUpRolesAsync(app.ApplicationServices));  // Process Asynchronously - no need to wait.
```

# Instantiating a Service Provider from a Service Collection

```csharp
// services => IServiceCollection
using (var serviceProvider = services.BuildServiceProvider())
{
}
```

# Helath Check
[Health checks in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2)

[How to set up ASP.NET Core 2.2 Health Checks with BeatPulse's AspNetCore.Diagnostics.HealthChecks](https://www.hanselman.com/blog/HowToSetUpASPNETCore22HealthChecksWithBeatPulsesAspNetCoreDiagnosticsHealthChecks.aspx)

https://blog.elmah.io/asp-net-core-2-2-health-checks-explained/


# Accessing DI Services
[Various Ways Of Accessing DI Services](http://www.binaryintellect.net/articles/17ee0ba2-99bb-47f0-ab18-f4fc32f476f8.aspx)
[ASP.NET CORE DEPENDENCY INJECTION � REGISTERING MULTIPLE IMPLEMENTATIONS OF AN INTERFACE](https://www.stevejgordon.co.uk/asp-net-core-dependency-injection-registering-multiple-implementations-interface)
[https://ardalis.com/how-to-list-all-services-available-to-an-asp-net-core-app](https://ardalis.com/how-to-list-all-services-available-to-an-asp-net-core-app)

# Miscellaneous
[Top 13 ASP.NET Core Features You Need to Know](https://stackify.com/asp-net-core-features/)

# Benchmarks
https://www.techempower.com/benchmarks**/**

# JWT Authentication

https://dzone.com/articles/global-authorization-filter-in-net-core-net-core-s

https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api

http://blogs.quovantis.com/json-web-token-jwt-with-web-api/

https://stackoverflow.com/questions/39079250/implementing-jwt-authentication-in-asp-net-webapi-using-microsoft-system-identit

https://stackoverflow.com/questions/55137980/create-jwt-token-in-c-sharp-asp-net-core-web-api

https://stackoverflow.com/questions/10055158/is-there-any-json-web-token-jwt-example-in-c

http://www.decatechlabs.com/secure-webapi-using-jwt

https://bitoftech.net/2014/10/27/json-web-token-asp-net-web-api-2-jwt-owin-authorization-server/

