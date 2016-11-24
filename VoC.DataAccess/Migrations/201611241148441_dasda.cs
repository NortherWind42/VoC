namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dasda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserHistory",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Username = c.String(),
                        RequestCounter = c.Int(nullable: false),
                        LastRequest = c.DateTime(nullable: false),
                        AverageTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.UserId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TranslationsRelation", "WordsRefId", "dbo.Words");
            DropForeignKey("dbo.TranslationsRelation", "LanguageRefId", "dbo.Languages");
            DropIndex("dbo.TranslationsRelation", new[] { "WordsRefId" });
            DropIndex("dbo.TranslationsRelation", new[] { "LanguageRefId" });
            DropTable("dbo.TranslationsRelation");
            DropTable("dbo.UserHistory");
            DropTable("dbo.Words");
            DropTable("dbo.Languages");
        }
    }
}
