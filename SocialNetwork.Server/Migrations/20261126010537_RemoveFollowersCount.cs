using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFollowersCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followers_count",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "followed_count",
                table: "users",
                newName: "FollowedCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FollowedCount",
                table: "users",
                newName: "followed_count");

            migrationBuilder.AddColumn<int>(
                name: "followers_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
