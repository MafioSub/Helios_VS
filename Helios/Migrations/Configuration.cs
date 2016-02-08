namespace Helios.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Helios.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Helios.Models.UsersContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            if (!Roles.RoleExists("Admin")) Roles.CreateRole("Admin");
            if (!WebSecurity.UserExists("admin")) WebSecurity.CreateUserAndAccount("admin", "zaqwsx123");
            if (!Roles.GetRolesForUser("admin").Contains("Admin")) Roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" });
        }
    }
}
