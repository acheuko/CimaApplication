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