using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeatsStoreYt.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueMediaAssetBlobStorageKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaAssets_BlobStorageKey",
                table: "MediaAssets",
                column: "BlobStorageKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaAssets_BlobStorageKey",
                table: "MediaAssets");
        }
    }
}
