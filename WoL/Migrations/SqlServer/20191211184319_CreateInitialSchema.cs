using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WoL.Migrations
{
    public partial class CreateInitialSchema : Migration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Argumente von öffentlichen Methoden validieren", Justification = "Auto-generated and typically called by the framework, won't be called with null")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Host",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hostname = table.Column<string>(maxLength: 255, nullable: true),
                    Caption = table.Column<string>(maxLength: 255, nullable: false),
                    MacAddress = table.Column<byte[]>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Host", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Host_Hostname",
                table: "Host",
                column: "Hostname",
                unique: true,
                filter: "[Hostname] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Host_MacAddress",
                table: "Host",
                column: "MacAddress",
                unique: true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Argumente von öffentlichen Methoden validieren", Justification = "Auto-generated and typically called by the framework, won't be called with null")]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Host");
        }
    }
}
