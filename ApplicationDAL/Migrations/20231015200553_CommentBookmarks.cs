using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDAL.Migrations
{
    /// <inheritdoc />
    public partial class CommentBookmarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "BookmarkedCommentsIds",
                table: "Users",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Bookmarks",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookmarkedCommentsIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Bookmarks",
                table: "Comments");
        }
    }
}
