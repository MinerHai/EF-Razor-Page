using System;
using Bogus;
using DBWeb;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace razor_ef.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });


            Randomizer.Seed = new Random(8675309);
            var fakeArtile = new Faker<Article>();
            fakeArtile.RuleFor(x => x.Title, f => f.Lorem.Sentence(5, 5));
            fakeArtile.RuleFor(x => x.Created, f => f.Date.Between(new DateTime(2021, 1, 1), new DateTime(2021, 12, 31)));
            fakeArtile.RuleFor(x => x.Content, f => f.Lorem.Paragraphs(1, 4));


            for (int i = 0; i < 150; i++)
            {
                Article article = fakeArtile.Generate();
                migrationBuilder.InsertData(
                    table: "articles",
                    columns: new[] { "Title", "Created", "Content" },
                    values: new object[] {
                    article.Title,
                    article.Created,
                    article.Content
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
