using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Programming.Migrations
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Mail = table.Column<string>(maxLength: 50, nullable: true),
                    Nick = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Synopsis = table.Column<string>(maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    SessionCode = table.Column<string>(maxLength: 20, nullable: true),
                    SessionEnd = table.Column<DateTime>(nullable: false),
                    VerificationCode = table.Column<string>(maxLength: 6, nullable: true),
                    VerificationEnd = table.Column<DateTime>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    UserImgURL = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
