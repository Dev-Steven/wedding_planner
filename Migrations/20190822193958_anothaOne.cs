using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wedding_planner.Migrations
{
    public partial class anothaOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(nullable: false),
                    last_name = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Weddings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    wedder_one = table.Column<string>(nullable: false),
                    wedder_two = table.Column<string>(nullable: false),
                    date = table.Column<string>(nullable: false),
                    address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weddings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weddings_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rsvps",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    wedding_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rsvps", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rsvps_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rsvps_Weddings_wedding_id",
                        column: x => x.wedding_id,
                        principalTable: "Weddings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_user_id",
                table: "Rsvps",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_wedding_id",
                table: "Rsvps",
                column: "wedding_id");

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_user_id",
                table: "Weddings",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rsvps");

            migrationBuilder.DropTable(
                name: "Weddings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
