using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;

namespace Cima.Repository
{
    public class REPO_Derogation : SqlBaseRepository<Derogation>
    {
        public REPO_Derogation()
        {
           
        }

        public REPO_Derogation(SysmanDbContext context) : base(context)
        {
            
        }

        public int GetNbOpenedDerogation()
        {
            int nb = 0;

            using (var sqlConnection = (SqlConnection)Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT COUNT(*) NbDerogation FROM sysman.tblDerogation WHERE Statut = 'O'", sqlConnection))
            {
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            nb = sqlQueryResult.GetInt32(0);
                        }

                    }
            }

            return nb;
        }
    }
}