using Cima.Models;
using System.Data.Entity;

namespace Cima.AppContext
{

    public class SysmanDbContext : DbContext
    {
        public SysmanDbContext() : base("name=SYSMAN_DS_CONNECTION")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SysmanDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<ProfilUser> ProfilUsers { get; set; }

        public DbSet<ProfilPrivilege> ProfilPrivileges{ get; set; }
    }
}