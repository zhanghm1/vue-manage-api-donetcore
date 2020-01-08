using Microsoft.EntityFrameworkCore.Migrations;

namespace VueManage.Infrastructure.EFCore.Migrations
{
    public partial class add_productnumber_orderitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductNumber",
                table: "UserOrderItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNumber",
                table: "UserOrderItem");
        }
    }
}
