using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KnowYourCustomer.Kyc.Host.Infrastructure.Migrations
{
    public partial class InitialKycDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kyc",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kyc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KycDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    KycId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Document = table.Column<byte[]>(nullable: true),
                    UploadedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KycOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    KycId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Operation = table.Column<int>(nullable: false),
                    Provider = table.Column<int>(nullable: false),
                    TimeOperation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycOperations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kyc");

            migrationBuilder.DropTable(
                name: "KycDocuments");

            migrationBuilder.DropTable(
                name: "KycOperations");
        }
    }
}
