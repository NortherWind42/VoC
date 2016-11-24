namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _213123 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Translations", "WordId", "dbo.Words");
            DropForeignKey("dbo.Translations", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Translations", new[] { "LanguageId" });
            DropIndex("dbo.Translations", new[] { "WordId" });
            CreateTable(
                "dbo.TranslationsRelation",
                c => new
                    {
                        LanguageRefId = c.Int(nullable: false),
                        WordsRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LanguageRefId, t.WordsRefId })
                .ForeignKey("dbo.Languages", t => t.LanguageRefId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordsRefId, cascadeDelete: true)
                .Index(t => t.LanguageRefId)
                .Index(t => t.WordsRefId);
            
            AddColumn("dbo.Languages", "Code", c => c.String(nullable: false));
            AddColumn("dbo.UserHistory", "Username", c => c.String());
            DropTable("dbo.Translations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(),
                        WordId = c.Int(),
                        Possibility = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TranslationsRelation", "WordsRefId", "dbo.Words");
            DropForeignKey("dbo.TranslationsRelation", "LanguageRefId", "dbo.Languages");
            DropIndex("dbo.TranslationsRelation", new[] { "WordsRefId" });
            DropIndex("dbo.TranslationsRelation", new[] { "LanguageRefId" });
            DropColumn("dbo.UserHistory", "Username");
            DropColumn("dbo.Languages", "Code");
            DropTable("dbo.TranslationsRelation");
            CreateIndex("dbo.Translations", "WordId");
            CreateIndex("dbo.Translations", "LanguageId");
            AddForeignKey("dbo.Translations", "LanguageId", "dbo.Languages", "Id");
            AddForeignKey("dbo.Translations", "WordId", "dbo.Words", "Id");
        }
    }
}
