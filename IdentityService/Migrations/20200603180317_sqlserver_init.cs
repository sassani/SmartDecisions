using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityService.Migrations
{
    public partial class sqlserver_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientPublicId = table.Column<string>(maxLength: 25, nullable: false),
                    ClientSecret = table.Column<string>(maxLength: 75, nullable: false),
                    Name = table.Column<string>(nullable: false),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 25, nullable: false),
                    Password = table.Column<string>(maxLength: 75, nullable: false),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(nullable: false),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                values: new object[] { 1, "cuv12t7", "$5$10$oURSNe5iuOVRE2EdVOVZgh$VoLOwW3wNnWJJRdW3yyUKe3H4dHmZK4LwR1ixbNZuow=", "localhost", 1 });

            migrationBuilder.InsertData(
                table: "Credential",
                columns: new[] { "Id", "Email", "IsActive", "IsEmailVerified", "LastLoginAt", "Password", "PublicId" },
                values: new object[] { 1, "a.sassani@gmail.com", true, true, null, "$5$10$4rGGwfd4YAzHNTdLxGYEH5$t7+5q42NVNAF8Wu5Y7f22Gjo9v35e6m4uOqLBigiGi8=", "abf243f7-9ac0-428b-b7b3-f14dc1979b65" });

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
