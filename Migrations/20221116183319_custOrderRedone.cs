using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crimson_closet.Migrations
{
    public partial class custOrderRedone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnByDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustOrder_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustOrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustOrderItem_CustOrder_CustOrderId",
                        column: x => x.CustOrderId,
                        principalTable: "CustOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustOrderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustOrder_ApplicationUserId",
                table: "CustOrder",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustOrderItem_CustOrderId",
                table: "CustOrderItem",
                column: "CustOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustOrderItem_ItemId",
                table: "CustOrderItem",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustOrderItem");

            migrationBuilder.DropTable(
                name: "CustOrder");
        }
    }
}
