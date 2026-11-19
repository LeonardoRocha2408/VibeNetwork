using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Server.Migrations
{
    /// <inheritdoc />
    public partial class FollowedCountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "followed_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followed_count",
                table: "users");
        }
    }
}
