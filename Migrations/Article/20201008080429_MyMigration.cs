using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Programming.Migrations.Article
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    IsTop = table.Column<bool>(nullable: false),
                    IsOriginal = table.Column<bool>(nullable: false),
                    IsPass = table.Column<bool>(nullable: false),
                    PassDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    KeyWord = table.Column<string>(nullable: true),
                    LookCount = table.Column<int>(nullable: false),
                    CommentCount = table.Column<int>(nullable: false),
                    Family = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
