using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatDog.Api.Migrations;

public partial class CreateAuthTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Nome = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                EmailConfirmedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                FailedLoginAttempts = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                LockedUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateTable(
            name: "confirmation_tokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                TokenHash = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UsedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_confirmation_tokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_confirmation_tokens_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "password_reset_tokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                TokenHash = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UsedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_password_reset_tokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_password_reset_tokens_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "refresh_tokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                TokenHash = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                Revoked = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                RevokedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                LastUsedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                DeviceInfo = table.Column<string>(type: "TEXT", nullable: true),
                ReplacedByTokenHash = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_refresh_tokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_refresh_tokens_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_confirmation_tokens_UserId",
            table: "confirmation_tokens",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_password_reset_tokens_UserId",
            table: "password_reset_tokens",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_refresh_tokens_UserId",
            table: "refresh_tokens",
            column: "UserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "confirmation_tokens");
        migrationBuilder.DropTable(name: "password_reset_tokens");
        migrationBuilder.DropTable(name: "refresh_tokens");
        migrationBuilder.DropTable(name: "users");
    }
}
