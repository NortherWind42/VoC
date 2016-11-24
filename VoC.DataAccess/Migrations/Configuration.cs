namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VoC.DataAccess.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VoC.DataAccess.MainContext context)
        {
            //  This method will be called after migrating to the latest version.
              context.Languages.AddOrUpdate( m=>m.Code,
								new Language { Name = "Английский", Code = "en" },
								new Language { Name = "Русский", Code = "ru" },
								new Language { Name = "Португальский", Code = "pt" },
								new Language { Name = "Болгарский", Code = "bg" },
								new Language { Name = "Испанский", Code = "es" }
							);
            context.SaveChanges();
        }
    }
}
