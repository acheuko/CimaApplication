using Cima.AppContext;
using Cima.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Repository.Shared
{
    public class UnitOfWork : IDisposable
    {
        private readonly SysmanDbContext context = new SysmanDbContext();

        private SqlBaseRepository<MenuItem> menuRepository;
        private SqlBaseRepository<ProfilUser> profilRepository;
        private SqlBaseRepository<ProfilPrivilege> profilPrivilegeRepository;
        private SqlBaseRepository<TblRule> ruleRepository;
        private SqlBaseRepository<TblReport> reportRepository;
        private SqlBaseRepository<TblAction> actionRepository;
        private SqlBaseRepository<TblDataColumn> datacolumnRepository;
        private SqlBaseRepository<TblRuleReport> rulereportRepository;
        private SqlBaseRepository<TblRuleBasedLog> rulebasedlogrepository;
        private SqlBaseRepository<TblRuleBasedDetail> rulebaseddetailrepository;
        private SqlBaseRepository<Icon> iconRepository;
        private SqlBaseRepository<User> userRepository;
        private SqlBaseRepository<Campaign> campaignRepository;
        private SqlBaseRepository<CampaignFile> campaignFileRepository;
        private SqlBaseRepository<CampaignCampaignFile> campaignCampaignFileRepository;
        private SqlBaseRepository<CampaignCampaignControl> campaignCampaignControlRepository;
        private SqlBaseRepository<Derogation> derogationRepository;
        private SqlBaseRepository<FichierRejete> fichierRejeteRepository;


        public SqlBaseRepository<Campaign> CampaignRepository
        {
            get
            {

                if (this.campaignRepository == null)
                {
                    this.campaignRepository = new REPO_Campaign(context);
                }
                return campaignRepository;
            }
        }

        public SqlBaseRepository<FichierRejete> FichierRejeteRepository
        {
            get
            {

                if (this.fichierRejeteRepository == null)
                {
                    this.fichierRejeteRepository = new REPO_FichierRejete(context);
                }
                return fichierRejeteRepository;
            }
        }

        public SqlBaseRepository<Derogation> DerogationRepository
        {
            get
            {

                if (this.derogationRepository == null)
                {
                    this.derogationRepository = new REPO_Derogation(context);
                }
                return derogationRepository;
            }
        }

        public SqlBaseRepository<CampaignFile> CampaignFileRepository
        {
            get
            {

                if (this.campaignFileRepository == null)
                {
                    this.campaignFileRepository = new REPO_CampaignFile(context);
                }
                return campaignFileRepository;
            }
        }

        public SqlBaseRepository<CampaignCampaignFile> CampaignCampaignFileRepository
        {
            get
            {

                if (this.campaignCampaignFileRepository == null)
                {
                    this.campaignCampaignFileRepository = new REPO_CampaignCampaignFile(context);
                }
                return campaignCampaignFileRepository;
            }
        }

        public SqlBaseRepository<CampaignCampaignControl> CampaignCampaignControlRepository
        {
            get
            {

                if (this.campaignCampaignControlRepository == null)
                {
                    this.campaignCampaignControlRepository = new REPO_CampaignCampaignControl(context);
                }
                return campaignCampaignControlRepository;
            }
        }


        public SqlBaseRepository<MenuItem> MenuRepository
        {
            get
            {

                if (this.menuRepository == null)
                {
                    this.menuRepository = new REPO_MenuItems(context);
                }
                return menuRepository;
            }
        }

        public SqlBaseRepository<ProfilUser> ProfilRepository
        {
            get
            {
                if (this.profilRepository == null)
                {
                    this.profilRepository = new REPO_ProfilUser(context);
                }
                return profilRepository;
            }
        }

        public SqlBaseRepository<ProfilPrivilege> ProfilPrivilegeRepository
        {
            get
            {
                if (this.profilPrivilegeRepository == null)
                {
                    this.profilPrivilegeRepository = new REPO_ProfilePrivilege(context);
                }
                return profilPrivilegeRepository;
            }
        }


        public SqlBaseRepository<TblRule> RuleRepository
        {
            get
            {
                if (this.ruleRepository == null)
                {
                    this.ruleRepository = new REPO_Rule(context);
                }
                return ruleRepository;
            }
        }

        public SqlBaseRepository<TblReport> ReportRepository
        {
            get
            {
                if (this.reportRepository == null)
                {
                    this.reportRepository = new REPO_Report(context);
                }
                return reportRepository;
            }
        }

        public SqlBaseRepository<TblAction> ActionRepository
        {
            get
            {
                if (this.actionRepository == null)
                {
                    this.actionRepository = new REPO_Action(context);
                }
                return actionRepository;
            }
        }

        public SqlBaseRepository<TblDataColumn> DataColumnRepository
        {
            get
            {
                if (this.datacolumnRepository == null)
                {
                    this.datacolumnRepository = new REPO_DataColumn(context);
                }
                return datacolumnRepository;
            }
        }

        public SqlBaseRepository<TblRuleReport> RuleReportRepository
        {
            get
            {
                if (this.rulereportRepository == null)
                {
                    this.rulereportRepository = new REPO_RuleReport(context);
                }
                return rulereportRepository;
            }
        }

        public SqlBaseRepository<TblRuleBasedLog> RuleBasedLogRepository
        {
            get
            {
                if (this.rulebasedlogrepository == null)
                {
                    this.rulebasedlogrepository = new REPO_RuleBasedLog(context);
                }
                return rulebasedlogrepository;
            }
        }

        public SqlBaseRepository<TblRuleBasedDetail> RuleBasedDetailRepository
        {
            get
            {
                if (this.rulebaseddetailrepository == null)
                {
                    this.rulebaseddetailrepository = new REPO_RuleBasedDetail(context);
                }
                return rulebaseddetailrepository;
             }
        }

        public SqlBaseRepository<Icon> IconRepository
        {
            get
            {
                if(this.iconRepository == null)
                {
                    this.iconRepository = new REPO_Icon(context);
                }
                return iconRepository;
            }
        }

        public SqlBaseRepository<User> UserRepository
        {
            get
            {
                if(this.userRepository == null)
                {
                    this.userRepository = new REPO_User(context);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}