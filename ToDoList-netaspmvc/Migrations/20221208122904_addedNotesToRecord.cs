using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList_netaspmvc.Migrations
{
    public partial class addedNotesToRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Record",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Record");
        }
    }
}
