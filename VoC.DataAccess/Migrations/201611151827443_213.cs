namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _213 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserHistory", "AverageTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserHistory", "AverageTime", c => c.DateTime(nullable: false));
        }
    }
}
