using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_ServiceNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceNotes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    note = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    item_instance_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    create_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    entity_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceNotes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceNotes");
        }
    }
}
