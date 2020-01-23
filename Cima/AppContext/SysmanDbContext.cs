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

        public DbSet<Icon> Icons { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<CampaignFile> CampaignFiles { get; set; }

        public DbSet<CampaignControl> CampaignControls { get; set; }

        public DbSet<CampaignCampaignControl> CampaignCampaignControl { get; set; }

        public DbSet<CampaignCampaignFile> CampaignCampaignFile { get; set; }

        public DbSet<Derogation> Derogations { get; set; }

        public DbSet<FichierRejete> FichierRejetes { get; set; }

    }
}