using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sjg.IdentityCore.Models;
using System;

// GUIDs -
// https://docs.microsoft.com/en-us/ef/core/modeling/generated-properties
//    GUID - Value generated on add -  when using SQL Server, values will be automatically
//           generated for GUID properties (using the SQL Server sequential GUID algorithm).

// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-2.2

// Many to Many
// https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-1-the-basics/
// https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-2-hiding-as-ienumerable/
// https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration

namespace Sjg.IdentityCore
{
    public class AccAuthContext : IdentityDbContext<
        AccAuthUser,
        AccAuthRole,
        Guid
        //// SJG - NOT NEEDED ??
        ////AccAuthUserClaim,
        //IdentityUserClaim<Guid>,
        //IdentityUserRole<Guid>,
        //IdentityUserLogin<Guid>,
        ////AccAuthRoleClaim,
        //IdentityRoleClaim<Guid>,
        //IdentityUserToken<Guid>
        >
    {
        public const string DefaultSchema = "Sjg.IdentityCore";

        //public DbSet<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }

        public DbSet<AccAuthGroup> AccAuthGroups { get; set; }
        public DbSet<AccAuthGroupRole> AccAuthGroupRoles { get; set; }
        public DbSet<AccAuthGroupUser> AccAuthGroupUsers { get; set; }
        public DbSet<AccAuthRole> AccessRoles { get; set; }
        public DbSet<AccAuthInvite> AccAuthInvites { get; set; }
        public DbSet<AccAuthInviteRole> AccAuthInviteRoles { get; set; }

        // In your context's constructors you don't need to do this anymore
        // public AccAuthContext(DbContextOptions<AccAuthContext> options) : base(options)
        // Just do:
        // public AccAuthContext(DbContextOptions options) : base(options)

        public AccAuthContext(DbContextOptions<AccAuthContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);

            // -- CONFIGURE Tables - Start

            // AccAuthUser
            builder.Entity<AccAuthUser>().HasIndex(b => b.LastName);
            builder.Entity<AccAuthUser>().HasIndex(b => b.FirstName);
            builder.Entity<AccAuthUser>().HasIndex(b => b.LastLoginDateTimeUtc);
            builder.Entity<AccAuthUser>().HasIndex(b => b.LastPasswordChangeDateTimeUtc);
            builder.Entity<AccAuthUser>().HasIndex(b => b.EmailDomainName);
            builder.Entity<AccAuthUser>().HasIndex(b => b.LastEmailConfirmedUtc);

            // AccessRole
            builder.Entity<AccAuthRole>().HasIndex(b => b.Description);
            builder.Entity<AccAuthRole>().HasIndex(b => b.Category);

            // AccAuthGroup
            builder.Entity<AccAuthGroup>().HasIndex(b => b.Group);
            builder.Entity<AccAuthGroup>().HasIndex(b => b.Description);
            builder.Entity<AccAuthGroup>().HasIndex(b => b.Category);

            // AccAuthGroupRole
            builder.Entity<AccAuthGroupRole>().HasKey(t => new { t.AccAuthGroupId, t.AccessRoleId });

            // AccAuthGroupUser
            builder.Entity<AccAuthGroupUser>().HasKey(t => new { t.AccAuthGroupId, t.AccAuthUserId });

            // AccAuthInvite
            builder.Entity<AccAuthInvite>().HasIndex(b => b.Email).IsUnique(true);
            builder.Entity<AccAuthInvite>().HasIndex(b => b.ExpirationDateUtc);
            builder.Entity<AccAuthInvite>().HasIndex(b => b.DisplayName);
            builder.Entity<AccAuthInvite>().HasIndex(b => b.Code);

            // AccAuthInviteRole
            builder.Entity<AccAuthInviteRole>().HasKey(t => new { t.AccAuthInviteId, t.AccessRoleId });

            // -- CONFIGURE Tables - Stop

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        //////protected override void OnConfiguring(DbContextOptionsBuilder options)
        //////{
        //////    options.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("__MyMigrationsHistory", "mySchema"));
        //////}
    }
}