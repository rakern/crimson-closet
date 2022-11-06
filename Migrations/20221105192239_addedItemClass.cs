using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class addedItemClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemStatus = table.Column<int>(type: "enum", nullable: true),
                    ItemBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemPhoto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ItemTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Item_ItemType_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "ItemType",
                        principalColumn: "ItemTypeID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemTypeID",
                table: "Item",
                column: "ItemTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
