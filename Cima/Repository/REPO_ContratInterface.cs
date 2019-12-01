using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;

namespace Cima.Repository
{
    public class REPO_ContratInterface : SqlBaseRepository<ContratInterface>
    {
        public REPO_ContratInterface()
        {
           
        }

        public REPO_ContratInterface(SysmanDbContext context) : base(context)
        {
           
        }

        public List<string> GetByFileMask(string FileMask)
        {

            List<string> listcolonnes = new List<string>();
            using (var sqlConnection = this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT Colonne FROM sysman.tblContratInterface WHERE Filemask = @ParamFilemask", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@ParamFilemask", FileMask);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        
                        while (sqlQueryResult.Read())
                        {
                            string Colonne = sqlQueryResult.GetString(0);
                            listcolonnes.Add(Colonne);                            
                        }

                    }
            }

            return listcolonnes;
        }
    }
}