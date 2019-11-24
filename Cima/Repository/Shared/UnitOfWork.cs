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
        private SqlBaseRepository<Icon> iconRepository;
        private SqlBaseRepository<User> userRepository;
        private SqlBaseRepository<Campaign> campaignRepository;
        private SqlBaseRepository<CampaignFile> campaignFileRepository;
        private SqlBaseRepository<CampaignCampaignFile> campaignCampaignFileRepository;
        private SqlBaseRepository<CampaignCampaignControl> campaignCampaignControlRepository;


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