using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Notifications.API.Migrations
{
    /// <inheritdoc />
    public partial class Messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    MessageBody = table.Column<string>(type: "text", nullable: false),
                    StorageTimeAfterSendingInHours = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    StoragePath = table.Column<string>(type: "text", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageRecipients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    ContactTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ContactData = table.Column<string>(type: "text", nullable: false),
                    DeliveryStatusId = table.Column<long>(type: "bigint", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    NextRetry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRecipients_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageRecipients_DeliveryStatuses_DeliveryStatusId",
                        column: x => x.DeliveryStatusId,
                        principalTable: "DeliveryStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageRecipients_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "Code", "CreatedAt", "DeletedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, 1, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Адрес электронной почты", false, "Email", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 2L, 2, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Номер телефона", false, "Phone", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 3L, 3, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Имя пользователя в Telegram", false, "Telegram", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) }
                });

            migrationBuilder.InsertData(
                table: "DeliveryStatuses",
                columns: new[] { "Id", "Code", "CreatedAt", "DeletedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, 1, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Сообщение добавлено в очередь", false, "В очереди", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 2L, 2, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Сообщение обрабатывается", false, "В обработке", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 3L, 3, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Сообщение успешно доставлено", false, "Отправлено", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 4L, 4, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Ошибка при отправке", false, "Ошибка", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) },
                    { 9999L, 9999, new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335), null, "Не поддерживается", false, "Не поддерживается", new DateTime(2026, 5, 7, 8, 38, 16, 289, DateTimeKind.Utc).AddTicks(3335) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_MessageId",
                table: "Attachment",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecipients_ContactTypeId",
                table: "MessageRecipients",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecipients_DeliveryStatusId",
                table: "MessageRecipients",
                column: "DeliveryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecipients_MessageId",
                table: "MessageRecipients",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "MessageRecipients");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "DeliveryStatuses");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
