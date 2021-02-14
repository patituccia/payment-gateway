using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Persistence.SqlLite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MerchantId = table.Column<int>(nullable: false),
                    CardHolderName = table.Column<string>(nullable: true),
                    MaskedCardNumber = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Money_Amount = table.Column<decimal>(nullable: true),
                    Money_Currency = table.Column<string>(nullable: true),
                    AcquiringBankPaymentId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Apple" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Microsoft" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Amazon" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantId",
                table: "Payments",
                column: "MerchantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
