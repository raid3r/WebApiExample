using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExample.Migrations
{
    /// <inheritdoc />
    public partial class UserTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ToDoListItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoListItems_UserId",
                table: "ToDoListItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoListItems_AspNetUsers_UserId",
                table: "ToDoListItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoListItems_AspNetUsers_UserId",
                table: "ToDoListItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoListItems_UserId",
                table: "ToDoListItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoListItems");
        }
    }
}
