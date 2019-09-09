using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Cima.Repository
{
    public class REPO_MenuItems : SqlBaseRepository<MenuItem>
    {
       
        public REPO_MenuItems(SysmanDbContext context):base(context)
        {
          
        }

    }
}