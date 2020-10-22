using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Programming.Migrations.DownloadItem
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsPass = table.Column<bool>(nullable: false),
                    PassDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Describe = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    DownloadCount = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadItems");
        }
    }
}
