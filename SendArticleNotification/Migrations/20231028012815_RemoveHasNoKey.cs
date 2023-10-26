using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SendArticleNotification.Migrations
{
    public partial class RemoveHasNoKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Emails",
                table: "Emails",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Emails",
                table: "Emails");
        }
    }
}
