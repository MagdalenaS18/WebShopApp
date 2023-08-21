using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApp.Migrations
{
    public partial class AddingSellerIdToOrderArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "OrderArticles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "OrderArticles");
        }
    }
}
