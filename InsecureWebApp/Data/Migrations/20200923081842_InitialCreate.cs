using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MicroFocus.InsecureWebApp.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    OnSale = table.Column<bool>(nullable: false),
                    SalePrice = table.Column<decimal>(nullable: true),
                    InStock = table.Column<bool>(nullable: false),
                    TimeToStock = table.Column<int>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductSearch",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    SearchText = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSearch", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    DocName = table.Column<string>(nullable: false),
                    Advice = table.Column<string>(nullable: false),
                    Product = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
            migrationBuilder.DropTable(
                name: "ProductSearch");
            migrationBuilder.DropTable(
                name: "Prescription");
            migrationBuilder.DropTable(
                name: "Order");
            migrationBuilder.DropTable(
                name: "OrderDetail");
        }
    }
}
