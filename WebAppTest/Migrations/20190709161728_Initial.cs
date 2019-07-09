using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppTest.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sjg.IdentityCore");

            migrationBuilder.CreateTable(
                name: "AccAuthGroups",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    AccAuthGroupId = table.Column<Guid>(nullable: false),
                    Group = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    Category = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccAuthGroups", x => x.AccAuthGroupId);
                });

            migrationBuilder.CreateTable(
                name: "AccAuthInvites",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    AccAuthInviteId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    ExpirationDateUtc = table.Column<DateTime>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: true),
                    IsServiceAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccAuthInvites", x => x.AccAuthInviteId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Category = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: true),
                    FirstName = table.Column<string>(maxLength: 75, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsServiceAccount = table.Column<bool>(nullable: false),
                    IsInternalServiceAccount = table.Column<bool>(nullable: false),
                    IsFrozen = table.Column<bool>(nullable: false),
                    PasswordNeverExpires = table.Column<bool>(nullable: false),
                    LastLoginDateTimeUtc = table.Column<DateTime>(nullable: true),
                    LastPasswordChangeDateTimeUtc = table.Column<DateTime>(nullable: true),
                    IsActiveDirectoryUser = table.Column<bool>(nullable: false),
                    EmailDomainName = table.Column<string>(maxLength: 256, nullable: true),
                    LastEmailConfirmedUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccAuthGroupRoles",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    AccAuthGroupId = table.Column<Guid>(nullable: false),
                    AccessRoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccAuthGroupRoles", x => new { x.AccAuthGroupId, x.AccessRoleId });
                    table.ForeignKey(
                        name: "FK_AccAuthGroupRoles_AccAuthGroups_AccAuthGroupId",
                        column: x => x.AccAuthGroupId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AccAuthGroups",
                        principalColumn: "AccAuthGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccAuthGroupRoles_AspNetRoles_AccessRoleId",
                        column: x => x.AccessRoleId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccAuthInviteRoles",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    AccAuthInviteId = table.Column<Guid>(nullable: false),
                    AccessRoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccAuthInviteRoles", x => new { x.AccAuthInviteId, x.AccessRoleId });
                    table.ForeignKey(
                        name: "FK_AccAuthInviteRoles_AccAuthInvites_AccAuthInviteId",
                        column: x => x.AccAuthInviteId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AccAuthInvites",
                        principalColumn: "AccAuthInviteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccAuthInviteRoles_AspNetRoles_AccessRoleId",
                        column: x => x.AccessRoleId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccAuthGroupUsers",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    AccAuthGroupId = table.Column<Guid>(nullable: false),
                    AccAuthUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccAuthGroupUsers", x => new { x.AccAuthGroupId, x.AccAuthUserId });
                    table.ForeignKey(
                        name: "FK_AccAuthGroupUsers_AccAuthGroups_AccAuthGroupId",
                        column: x => x.AccAuthGroupId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AccAuthGroups",
                        principalColumn: "AccAuthGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccAuthGroupUsers_AspNetUsers_AccAuthUserId",
                        column: x => x.AccAuthUserId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "Sjg.IdentityCore",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Sjg.IdentityCore",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthGroupRoles_AccessRoleId",
                schema: "Sjg.IdentityCore",
                table: "AccAuthGroupRoles",
                column: "AccessRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthGroups_Category",
                schema: "Sjg.IdentityCore",
                table: "AccAuthGroups",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthGroups_Description",
                schema: "Sjg.IdentityCore",
                table: "AccAuthGroups",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthGroups_Group",
                schema: "Sjg.IdentityCore",
                table: "AccAuthGroups",
                column: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthGroupUsers_AccAuthUserId",
                schema: "Sjg.IdentityCore",
                table: "AccAuthGroupUsers",
                column: "AccAuthUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthInviteRoles_AccessRoleId",
                schema: "Sjg.IdentityCore",
                table: "AccAuthInviteRoles",
                column: "AccessRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthInvites_Code",
                schema: "Sjg.IdentityCore",
                table: "AccAuthInvites",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthInvites_DisplayName",
                schema: "Sjg.IdentityCore",
                table: "AccAuthInvites",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthInvites_Email",
                schema: "Sjg.IdentityCore",
                table: "AccAuthInvites",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccAuthInvites_ExpirationDateUtc",
                schema: "Sjg.IdentityCore",
                table: "AccAuthInvites",
                column: "ExpirationDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Sjg.IdentityCore",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Category",
                schema: "Sjg.IdentityCore",
                table: "AspNetRoles",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Description",
                schema: "Sjg.IdentityCore",
                table: "AspNetRoles",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Sjg.IdentityCore",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Sjg.IdentityCore",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Sjg.IdentityCore",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Sjg.IdentityCore",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmailDomainName",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "EmailDomainName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FirstName",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastEmailConfirmedUtc",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "LastEmailConfirmedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastLoginDateTimeUtc",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "LastLoginDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastName",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastPasswordChangeDateTimeUtc",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "LastPasswordChangeDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Sjg.IdentityCore",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccAuthGroupRoles",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AccAuthGroupUsers",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AccAuthInviteRoles",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AccAuthGroups",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AccAuthInvites",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "Sjg.IdentityCore");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "Sjg.IdentityCore");
        }
    }
}
