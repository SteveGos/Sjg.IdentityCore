IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AccAuthGroupRoles];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AccAuthGroupUsers];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AccAuthInviteRoles];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetRoleClaims];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetUserClaims];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetUserLogins];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetUserRoles];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetUserTokens];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AccAuthGroups];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AccAuthInvites];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetRoles];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DROP TABLE [Sjg.IdentityCore].[AspNetUsers];
END;

GO

IF EXISTS(SELECT * FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore] WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0')
BEGIN
    DELETE FROM [Sjg.IdentityCore].[__MigrationsHistory_Sjg_IdentityCore]
    WHERE [MigrationId] = N'20191023050239_Sjg.IdentityCore.1.0.0';
END;

GO

