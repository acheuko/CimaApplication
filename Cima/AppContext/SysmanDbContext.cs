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

        public DbSet<TblRule> TblRules { get; set; }

        public DbSet<TblAction> TblAction { get; set; }

        public DbSet<TblReport> TblReport { get; set; }

        public DbSet<TblDataColumn> TblDataColumn { get; set; }

        public DbSet<TblRuleReport> TblRuleReport { get; set; }

        public DbSet<TblRuleBasedLog> tblrulebasedlog { get; set; }

        public DbSet<TblRuleBasedDetail> tblrulebaseddetail { get; set; }
    }
}