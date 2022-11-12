using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class addOrderItemized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItemized",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemized", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemized_CustOrder_CustOrderId",
                        column: x => x.CustOrderId,
                        principalTable: "CustOrder",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItemized_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemized_CustOrderId",
                table: "OrderItemized",
                column: "CustOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemized_ItemId",
                table: "OrderItemized",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemized");
        }
    }
}
