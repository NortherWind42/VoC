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


            context.Languages.AddOrUpdate(
              p => p.Code,
               new Language { Name = "����������", Code = "en" },
                new Language { Name = "�������", Code = "ru" },
                new Language { Name = "�������������", Code = "pt" },
                new Language { Name = "����������", Code = "bg" },
                new Language { Name = "���������", Code = "es" }
            );
            context.SaveChanges();
        }
    }
}
