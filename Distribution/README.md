# Releases

Sjg.Identity Core is released as a NUGET package.  

- NUGET Packages are in Version folders.
- SQL Scripts will be available in the Version Folders as well.  


## Package Versions

A specific version number is in the form Major.Minor.Patch[-Suffix], where the components have the following meanings:

- *Major*: Breaking changes
- *Minor*: New features, but backwards compatible
- *Patch*: Backwards compatible bug fixes only -Suffix (optional): a hyphen followed by a string denoting a pre-release version (following the Semantic Versioning or SemVer 1.0 convention).
 
## SQL Scripts

This application uses EF Migrations. SQL scripts with versions corresponding to the NUGET package.

**Example**.

If you are installing Sjg.IdentityCore.2.5.2.nupkg, then you should use Sjg.IdentityCore.2.5.2.sql.

If the version does not have any SQL changes, the SQL package that should be applied is the package with the next lowest release.

**Example**.

If you are installing Sjg.IdentityCore.2.6.1.nupkg and there is **NOT** a corresponding SQL package, the next lower version SQL script should be used.
  
If the next lowest package is Sjg.IdentityCore.2.5.2.sql, then that is the one that is need for Sjg.IdentityCore.2.6.1.nupkg.

### Removing Changes

There will be a **REMOVE** script to back changes off if it is needed.

**Example**.

If **Sjg.IdentityCore.2.5.2.sql** needs to be rolled back, **Sjg.IdentityCore.2.5.2.Remove.sql** will remove the changes that were applied.

