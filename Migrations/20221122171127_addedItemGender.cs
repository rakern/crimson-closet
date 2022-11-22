using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class addedItemGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemGender",
                table: "Item",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemGender",
                table: "Item");
        }
    }
}
