using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList_netaspmvc.Migrations
{
    public partial class addedNotificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    toDoListId = table.Column<int>(nullable: false),
                    recordId = table.Column<int>(nullable: false),
                    recordTitle = table.Column<string>(nullable: true),
                    recordDescription = table.Column<string>(nullable: true),
                    DueDate = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    TimesLeftToRead = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}
