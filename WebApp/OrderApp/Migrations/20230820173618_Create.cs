using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApp.Migrations
{
    public partial class Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    MaxQuantity = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderArticles",
                columns: table => new
                {
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ArticleQuantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderArticles", x => new { x.OrderId, x.ArticleId });
                    table.ForeignKey(
                        name: "FK_OrderArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderArticles_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Name",
                table: "Articles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderArticles_ArticleId",
                table: "OrderArticles",
                column: "ArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderArticles");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
