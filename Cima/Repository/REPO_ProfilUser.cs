using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cima.Repository
{
    public class REPO_ProfilUser : SqlBaseRepository<ProfilUser>
    {

        public REPO_ProfilUser(SysmanDbContext context) : base(context)
        {

        }

    }
}