using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheLineSportsOnline.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public long Wallet { get; set; }
        public long MinWager { get; set; }
        public long MaxWager { get; set; }
        public bool Locked { get; set; }

        public long getMaxWager()
        {
            if (this.Wallet <= -1000)
            {
                return 0;
            }
            else if ((this.Wallet + 1000) > Math.Abs(this.Wallet - 1000))
            {
                return (this.Wallet + 1000);
            }
            else
            {
                return Math.Abs(this.Wallet - 1000);
            }
        }

        public long getMinWager()
        {
            if (this.Wallet > 0)
            {
                if ((this.Wallet / 2) % 10 != 0)
                {
                    return ((this.Wallet / 2) + 5);
                }
                else
                {
                    return (this.Wallet / 2);
                }
            }
            else
            {
                return 0;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Wager> Wagers { get; set; }
        public DbSet<LockOfTheWeek> Locks { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //    public System.Data.Entity.DbSet<TheLineSportsOnline.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
    public class IdentityManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }


        //public void ClearUserRoles(string userId)
        //{
        //    var um = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var user = um.FindById(userId);
        //    var currentRoles = new List<IdentityUserRole>();
        //    currentRoles.AddRange(user.Roles);
        //    foreach (var role in currentRoles)
        //    {
        //        um.RemoveFromRole(userId, role.Role.Name);
        //    }
        //}
    }
}