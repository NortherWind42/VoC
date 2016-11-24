namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(),
                        WordId = c.Int(),
                        Possibility = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Words", t => t.WordId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.WordId);
            
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
                        RequestCounter = c.Int(nullable: false),
                        LastRequest = c.DateTime(nullable: false),
                        AverageTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Translations", "WordId", "dbo.Words");
            DropIndex("dbo.Translations", new[] { "WordId" });
            DropIndex("dbo.Translations", new[] { "LanguageId" });
            DropTable("dbo.UserHistory");
            DropTable("dbo.Words");
            DropTable("dbo.Translations");
            DropTable("dbo.Languages");
        }
    }
}
