using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadmoreWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContactReplySeenFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SeenByAdmin",
                table: "ContactReplies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeenByUser",
                table: "ContactReplies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenByAdmin",
                table: "ContactReplies");

            migrationBuilder.DropColumn(
                name: "SeenByUser",
                table: "ContactReplies");
        }
    }
}
