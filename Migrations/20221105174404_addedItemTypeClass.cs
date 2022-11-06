using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class addedItemTypeClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    ItemTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.ItemTypeID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemType");
        }
    }
}
