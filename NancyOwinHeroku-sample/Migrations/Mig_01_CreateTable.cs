
using FluentMigrator;

namespace NancyOwinHeroku_sample.Migrations
{
    [Migration(1)]
    public class Mig_01_CreateTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Ringo")
                .WithColumn("ID").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString();

            Insert.IntoTable("Ringo")
                .Row(new { ID = 1, Name = "ふじ" })
                .Row(new { ID = 2, Name = "シナノゴールド" })
                .Row(new { ID = 3, Name = "あいかの香り" });
        }
    }
}
