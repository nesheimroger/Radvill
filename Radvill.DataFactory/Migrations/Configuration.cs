using System.Web.Mvc;
using Radvill.Models.AdviceModels;
using Radvill.Services.Security;

namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Radvill.DataFactory.Internal.Components.RadvillContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Radvill.DataFactory.Internal.Components.RadvillContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Categories.AddOrUpdate(x => x.ID, 
                new Category{ID = 1, Name = "Generelt"},
                new Category{ID = 2, Name = "Spesielt"}
            );
            
        }
    }
}
