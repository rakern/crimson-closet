using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class addedStatusToCustOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CustOrder",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CustOrder");
        }
    }
}
