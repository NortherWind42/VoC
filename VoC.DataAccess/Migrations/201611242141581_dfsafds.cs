namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfsafds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TranslationsRelation", "LanguageRefId", "dbo.Languages");
            DropForeignKey("dbo.TranslationsRelation", "WordsRefId", "dbo.Words");
            DropIndex("dbo.TranslationsRelation", new[] { "LanguageRefId" });
            DropIndex("dbo.TranslationsRelation", new[] { "WordsRefId" });
            CreateTable(
                "dbo.WordTranslations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Probability = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.WordId)
                .Index(t => t.LanguageId);
            
            DropTable("dbo.TranslationsRelation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TranslationsRelation",
                c => new
                    {
                        LanguageRefId = c.Int(nullable: false),
                        WordsRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LanguageRefId, t.WordsRefId });
            
            DropForeignKey("dbo.WordTranslations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.WordTranslations", "WordId", "dbo.Words");
            DropIndex("dbo.WordTranslations", new[] { "LanguageId" });
            DropIndex("dbo.WordTranslations", new[] { "WordId" });
            DropTable("dbo.WordTranslations");
            CreateIndex("dbo.TranslationsRelation", "WordsRefId");
            CreateIndex("dbo.TranslationsRelation", "LanguageRefId");
            AddForeignKey("dbo.TranslationsRelation", "WordsRefId", "dbo.Words", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TranslationsRelation", "LanguageRefId", "dbo.Languages", "Id", cascadeDelete: true);
        }
    }
}
