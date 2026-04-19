using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetailStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    HiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    DiscountId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gadgets and electronic devices", "Electronics" },
                    { 2, "Apparel and accessories", "Clothing" },
                    { 3, "Groceries and consumables", "Food" },
                    { 4, "Sports equipment and gear", "Sports" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Code", "ExpiresAt", "Percentage" },
                values: new object[,]
                {
                    { 1, "SUMMER10", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10.0 },
                    { 2, "SALE25", new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), 25.0 },
                    { 3, "FLASH50", new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 50.0 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FirstName", "HiredAt", "LastName", "Role" },
                values: new object[,]
                {
                    { 1, "Alice", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Johnson", "Manager" },
                    { 2, "Bob", new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Smith", "Cashier" },
                    { 3, "Carol", new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Davis", "Stock" },
                    { 4, "David", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Wilson", "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "DiscountId", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, 1, "Laptop Pro 15", 1299.99m, 20 },
                    { 2, 1, null, "Wireless Earbuds", 89.99m, 150 },
                    { 3, 1, 2, "Smart Watch", 249.99m, 60 },
                    { 4, 2, 3, "Running Jacket", 69.99m, 80 },
                    { 5, 2, null, "Classic T-Shirt", 24.99m, 200 },
                    { 6, 3, null, "Organic Coffee 1kg", 15.49m, 300 },
                    { 7, 3, 1, "Protein Bars (12x)", 19.99m, 180 },
                    { 8, 4, null, "Tennis Racket", 119.99m, 45 },
                    { 9, 4, 2, "Yoga Mat", 39.99m, 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Code",
                table: "Discounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountId",
                table: "Products",
                column: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Discounts");
        }
    }
}
