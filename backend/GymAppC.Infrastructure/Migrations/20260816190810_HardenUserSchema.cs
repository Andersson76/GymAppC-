using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HardenUserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE [Users]
                SET [Name] = LEFT(
                        CASE
                            WHEN [Name] IS NULL OR LTRIM(RTRIM([Name])) = ''
                                THEN CONCAT('User ', [Id])
                            ELSE LTRIM(RTRIM([Name]))
                        END,
                        100),
                    [Email] = LEFT(LOWER(LTRIM(RTRIM(COALESCE([Email], '')))), 256),
                    [Role] = CASE
                        WHEN LOWER(LTRIM(RTRIM(COALESCE([Role], '')))) = 'admin' THEN 'Admin'
                        ELSE 'User'
                    END;

                UPDATE [Users]
                SET [Email] = CONCAT(
                    'missing-', [Id], '-', LOWER(CONVERT(varchar(36), NEWID())), '@migration.invalid')
                WHERE [Email] = '';

                ;WITH [RankedUsers] AS
                (
                    SELECT [Id],
                           ROW_NUMBER() OVER (PARTITION BY [Email] ORDER BY [Id]) AS [EmailRank]
                    FROM [Users]
                )
                UPDATE [Users]
                SET [Email] = CONCAT(
                    'duplicate-', [Users].[Id], '-', LOWER(CONVERT(varchar(36), NEWID())), '@migration.invalid')
                FROM [Users]
                INNER JOIN [RankedUsers] ON [RankedUsers].[Id] = [Users].[Id]
                WHERE [RankedUsers].[EmailRank] > 1;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
