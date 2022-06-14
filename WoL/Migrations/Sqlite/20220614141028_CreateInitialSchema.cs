using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoL.Migrations.Sqlite
{
    public partial class CreateInitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Host",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hostname = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Caption = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MacAddress = table.Column<byte[]>(type: "BLOB", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Host", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Host_Hostname",
                table: "Host",
                column: "Hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Host_MacAddress",
                table: "Host",
                column: "MacAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Host");
        }
    }
}
