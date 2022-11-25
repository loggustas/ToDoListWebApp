using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList_netaspmvc.Migrations
{
    public partial class AddedDescriptionToDoList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ToDoList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ToDoList");
        }
    }
}
