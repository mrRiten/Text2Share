using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailMicroService.API.Migrations
{
    /// <inheritdoc />
    public partial class email : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Emails");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Emails");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
