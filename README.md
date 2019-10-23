# Sjg.IdentityCore
Identity Framework - Extended/Enhanced

This is a razor class library that extends / enhances `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.  

It adds on the ability to 

- manage users, 
- allow the site to self register or register by invitation only.
- Create a custom user authentication provider to authenticate users.  Authorization is still done with Identity Framework
  - The custom authenticator provider can be used with Identity Framework's Authentication or instead of Identity Framework's Authentication
- Contains an ApiBasicAuthorizeAttribute (ActionFilterAttribute) for basic authentication over HTTPS.

# Solution Contents

- SampleWebApp_1_x_x.  Sample web application that uses Sjg.IdentityCore.
- LdapActiveDirectoryHelper. Active Directory helper that can be utilized for LDAP/Active Directory authentication.
  - See the Providers folder in SampleWebApp_1_x_x
  - Custom Authentication Provider is a service that is added for Dependency Injection.  See IdentityHostingStartup.cs.
- Test API. Console Program that can be used to test API Basic Authorization.
- 

