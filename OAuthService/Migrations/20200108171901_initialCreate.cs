using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OAuthService.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientPublicId = table.Column<string>(maxLength: 25, nullable: false),
                    ClientSecret = table.Column<string>(maxLength: 75, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PublicId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 25, nullable: false),
                    Password = table.Column<string>(maxLength: 75, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    LastLoginAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logsheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefreshToken = table.Column<string>(nullable: true),
                    CredentialId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Platform = table.Column<string>(nullable: true),
                    Browser = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logsheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logsheet_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logsheet_Credential_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CredentialRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CredentialId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialRole_Credential_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CredentialRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "ClientPublicId", "ClientSecret", "Name", "Type" },
                values: new object[] { 1, "cuv12t7", "$5$10$mFiFWP2TGzuYtOEcH6ymaB$L48XHvOZDumRmt2euRv9b2pRJ93pOOMLsgIPAmAyBsg=", "localhost", 1 });

            migrationBuilder.InsertData(
                table: "Credential",
                columns: new[] { "Id", "Email", "IsActive", "IsEmailVerified", "LastLoginAt", "Password", "PublicId" },
                values: new object[] { 1, "a.sassani@gmail.com", true, true, null!, "$5$10$dGxTx7tECwnupyQxo0iAGN$6EwyCSDswb2JBJUZoKQQUqGCxdkU43SiMszlbglV59k=", "41e6655b-cf0b-4c37-8239-07f62f11b266" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "CredentialRole",
                columns: new[] { "Id", "CredentialId", "RoleId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_PublicId",
                table: "Credential",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRole_CredentialId",
                table: "CredentialRole",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRole_RoleId",
                table: "CredentialRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Logsheet_ClientId",
                table: "Logsheet",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Logsheet_CredentialId",
                table: "Logsheet",
                column: "CredentialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialRole");

            migrationBuilder.DropTable(
                name: "Logsheet");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Credential");
        }
    }
}
