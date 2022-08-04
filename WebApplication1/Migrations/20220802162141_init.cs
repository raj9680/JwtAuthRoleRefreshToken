using Microsoft.EntityFrameworkCore.Migrations;

namespace JwtAuthentication.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblCustomerr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCustomerr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblRefreshtokenn",
                columns: table => new
                {
                    TokenId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblRefreshtokenn", x => x.TokenId);
                });

            migrationBuilder.CreateTable(
                name: "TblUserr",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserr", x => x.Userid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblCustomerr");

            migrationBuilder.DropTable(
                name: "TblRefreshtokenn");

            migrationBuilder.DropTable(
                name: "TblUserr");
        }
    }
}
