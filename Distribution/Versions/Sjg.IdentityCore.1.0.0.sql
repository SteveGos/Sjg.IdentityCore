IF OBJECT_ID(N'[Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore]') IS NULL
BEGIN
    IF SCHEMA_ID(N'Sjg.IdentityCore') IS NULL EXEC(N'CREATE SCHEMA [Sjg.IdentityCore];');
    CREATE TABLE [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistory_Sjg_IdentityCore] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    IF SCHEMA_ID(N'Sjg.IdentityCore') IS NULL EXEC(N'CREATE SCHEMA [Sjg.IdentityCore];');
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AccAuthGroups] (
        [AccAuthGroupId] uniqueidentifier NOT NULL,
        [Group] nvarchar(256) NOT NULL,
        [Description] nvarchar(256) NOT NULL,
        [Category] nvarchar(256) NULL,
        CONSTRAINT [PK_AccAuthGroups] PRIMARY KEY ([AccAuthGroupId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AccAuthInvites] (
        [AccAuthInviteId] uniqueidentifier NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [ExpirationDateUtc] datetime2 NOT NULL,
        [DisplayName] nvarchar(100) NOT NULL,
        [Code] nvarchar(128) NULL,
        [IsServiceAccount] bit NOT NULL,
        [PasswordNeverExpires] bit NOT NULL,
        CONSTRAINT [PK_AccAuthInvites] PRIMARY KEY ([AccAuthInviteId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Category] nvarchar(256) NULL,
        [Description] nvarchar(256) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        [LastName] nvarchar(75) NULL,
        [FirstName] nvarchar(75) NULL,
        [IsActive] bit NOT NULL,
        [IsServiceAccount] bit NOT NULL,
        [IsInternalServiceAccount] bit NOT NULL,
        [IsFrozen] bit NOT NULL,
        [PasswordNeverExpires] bit NOT NULL,
        [LastLoginDateTimeUtc] datetime2 NULL,
        [LastPasswordChangeDateTimeUtc] datetime2 NULL,
        [IsCustomAuthenticated] bit NOT NULL,
        [EmailDomainName] nvarchar(256) NULL,
        [LastEmailConfirmedUtc] datetime2 NULL,
        [TwoStepAuthenticationEnabled] bit NOT NULL,
        [TwoStepAuthenticationCodeHash] nvarchar(max) NULL,
        [TwoStepAuthenticationCodeHashExpireUtc] datetime2 NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AccAuthGroupRoles] (
        [AccAuthGroupId] uniqueidentifier NOT NULL,
        [AccessRoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AccAuthGroupRoles] PRIMARY KEY ([AccAuthGroupId], [AccessRoleId]),
        CONSTRAINT [FK_AccAuthGroupRoles_AccAuthGroups_AccAuthGroupId] FOREIGN KEY ([AccAuthGroupId]) REFERENCES [Sjg.IdentityCore].[AccAuthGroups] ([AccAuthGroupId]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccAuthGroupRoles_AspNetRoles_AccessRoleId] FOREIGN KEY ([AccessRoleId]) REFERENCES [Sjg.IdentityCore].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AccAuthInviteRoles] (
        [AccAuthInviteId] uniqueidentifier NOT NULL,
        [AccessRoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AccAuthInviteRoles] PRIMARY KEY ([AccAuthInviteId], [AccessRoleId]),
        CONSTRAINT [FK_AccAuthInviteRoles_AccAuthInvites_AccAuthInviteId] FOREIGN KEY ([AccAuthInviteId]) REFERENCES [Sjg.IdentityCore].[AccAuthInvites] ([AccAuthInviteId]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccAuthInviteRoles_AspNetRoles_AccessRoleId] FOREIGN KEY ([AccessRoleId]) REFERENCES [Sjg.IdentityCore].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Sjg.IdentityCore].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AccAuthGroupUsers] (
        [AccAuthGroupId] uniqueidentifier NOT NULL,
        [AccAuthUserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AccAuthGroupUsers] PRIMARY KEY ([AccAuthGroupId], [AccAuthUserId]),
        CONSTRAINT [FK_AccAuthGroupUsers_AccAuthGroups_AccAuthGroupId] FOREIGN KEY ([AccAuthGroupId]) REFERENCES [Sjg.IdentityCore].[AccAuthGroups] ([AccAuthGroupId]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccAuthGroupUsers_AspNetUsers_AccAuthUserId] FOREIGN KEY ([AccAuthUserId]) REFERENCES [Sjg.IdentityCore].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Sjg.IdentityCore].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Sjg.IdentityCore].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Sjg.IdentityCore].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Sjg.IdentityCore].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE TABLE [Sjg.IdentityCore].[AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Sjg.IdentityCore].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthGroupRoles_AccessRoleId] ON [Sjg.IdentityCore].[AccAuthGroupRoles] ([AccessRoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthGroups_Category] ON [Sjg.IdentityCore].[AccAuthGroups] ([Category]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthGroups_Description] ON [Sjg.IdentityCore].[AccAuthGroups] ([Description]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthGroups_Group] ON [Sjg.IdentityCore].[AccAuthGroups] ([Group]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthGroupUsers_AccAuthUserId] ON [Sjg.IdentityCore].[AccAuthGroupUsers] ([AccAuthUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthInviteRoles_AccessRoleId] ON [Sjg.IdentityCore].[AccAuthInviteRoles] ([AccessRoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthInvites_Code] ON [Sjg.IdentityCore].[AccAuthInvites] ([Code]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthInvites_DisplayName] ON [Sjg.IdentityCore].[AccAuthInvites] ([DisplayName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE UNIQUE INDEX [IX_AccAuthInvites_Email] ON [Sjg.IdentityCore].[AccAuthInvites] ([Email]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AccAuthInvites_ExpirationDateUtc] ON [Sjg.IdentityCore].[AccAuthInvites] ([ExpirationDateUtc]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [Sjg.IdentityCore].[AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetRoles_Category] ON [Sjg.IdentityCore].[AspNetRoles] ([Category]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetRoles_Description] ON [Sjg.IdentityCore].[AspNetRoles] ([Description]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [Sjg.IdentityCore].[AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [Sjg.IdentityCore].[AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [Sjg.IdentityCore].[AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [Sjg.IdentityCore].[AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_EmailDomainName] ON [Sjg.IdentityCore].[AspNetUsers] ([EmailDomainName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_FirstName] ON [Sjg.IdentityCore].[AspNetUsers] ([FirstName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_LastEmailConfirmedUtc] ON [Sjg.IdentityCore].[AspNetUsers] ([LastEmailConfirmedUtc]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_LastLoginDateTimeUtc] ON [Sjg.IdentityCore].[AspNetUsers] ([LastLoginDateTimeUtc]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_LastName] ON [Sjg.IdentityCore].[AspNetUsers] ([LastName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [IX_AspNetUsers_LastPasswordChangeDateTimeUtc] ON [Sjg.IdentityCore].[AspNetUsers] ([LastPasswordChangeDateTimeUtc]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE INDEX [EmailIndex] ON [Sjg.IdentityCore].[AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [Sjg.IdentityCore].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    INSERT INTO [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] ([MigrationId], [ProductVersion])
    VALUES (N'20191023050239_Sjg.IdentityCore.1.0.0', N'2.2.6-servicing-10079');
END;

GO

