using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDAL.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteConfigForImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Comments_CommentId",
                table: "Images"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Images_Comments_CommentId",
                table: "Images",
                column: "CommentId",
                principalTable:"Comments",
                onDelete: ReferentialAction.Cascade
            );
            
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images",
                column: "PostId",
                principalTable:"Posts",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Comments_CommentId",
                table: "Images"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Images_Comments_CommentId",
                table: "Images",
                column: "CommentId",
                principalTable:"Comments",
                onDelete: ReferentialAction.Restrict
            );
            
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images",
                column: "PostId",
                principalTable:"Posts",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
