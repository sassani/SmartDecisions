using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientPublicId = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ClientSecret = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logsheet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    CredentialId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Platform = table.Column<string>(type: "text", nullable: true),
                    Browser = table.Column<string>(type: "text", nullable: true),
                    IP = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CredentialId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
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
                values: new object[] { 1, "cuv12t7", "$5$10$ckhN1j8davu7k19Gg5Dss5$5AHipwn9LpYTxw79RaUvERh2uGgL6M2K5tcRSpqJYuE=", "localhost", 1 });

            migrationBuilder.InsertData(
                table: "Credential",
                columns: new[] { "Id", "Email", "IsActive", "IsEmailVerified", "LastLoginAt", "Password", "PublicId" },
                values: new object[] { 1, "a.sassani@gmail.com", true, true, null, "$5$10$ckhN1j8davu7k19Gg5Dss5$96slsRntFoof8tCQmbWANa+6Y3Kdp2JzK+0f+Dgzfnw=", "8b3ff889-bd90-4232-ac76-188ec0289b35" });

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

        /// <inheritdoc />
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
