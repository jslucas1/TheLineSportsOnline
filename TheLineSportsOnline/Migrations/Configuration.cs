namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TheLineSportsOnline.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TheLineSportsOnline.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TheLineSportsOnline.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            AddUserAndRoles();
        }

        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();

            if (!idManager.RoleExists("Admin"))
            {
                success = idManager.CreateRole("Admin");
                if (!success)
                {
                    return success;
                }
            }

            if (!idManager.RoleExists("User"))
            {
                success = idManager.CreateRole("User");
                if (!success)
                {
                    return success;
                }
            }

            var adminUser = new ApplicationUser()
            {
                Email = "admin@email.com",
                UserName = "Admin"
            };

            var austin = new ApplicationUser()
            {
                Email = "Austin@mail.com",
                UserName = "Austin.Lucas"
            };

            success = idManager.CreateUser(adminUser, "#RTRadmin!");
            if (!success) return success;

            success = idManager.AddUserToRole(adminUser.Id, "Admin");
            if (!success) return success;



            success = idManager.CreateUser(austin, "Rolltide1");
            if (!success) return success;

            success = idManager.AddUserToRole(austin.Id, "Admin");
            if (!success) return success;

            success = idManager.AddUserToRole(austin.Id, "User");
            if (!success) return success;

            //add any other roles here

            //success = idManager.AddUserToRole(newUser.Id, "User");
            //if (!success) return success;

            return success;
        }
    }
}
