using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cima.Repository
{
    public class REPO_Rule : SqlBaseRepository<TblRule>
    {
        public REPO_Rule(SysmanDbContext context)
            : base(context)
        {

        }
    }
        
}