using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList_netaspmvc.Migrations
{
    public partial class updatedNotificationTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesLeftToRead",
                table: "Notification");

            migrationBuilder.AddColumn<string>(
                name: "DateToRemind",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateToRemind",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "TimesLeftToRead",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
