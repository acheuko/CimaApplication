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
    public class REPO_CampaignFile : SqlBaseRepository<CampaignFile>
    {

        public REPO_CampaignFile(SysmanDbContext context) : base(context)
        {

        }

        public ObservableCollection<CampaignFile> GetAll()
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<CampaignFile> items = new ObservableCollection<CampaignFile>();
            using (var sqlQuery = new SqlCommand(@"SELECT ID_FileMask, Libelle FROM sysman.tblFileMask ORDER BY Libelle", con))
            {
               
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {

                        while (sqlQueryResult.Read())
                        {
                            try
                            {
                                items.Add(this.MapItem(sqlQueryResult));
                            }
                            catch
                            {
                                sqlQueryResult.Close();
                                con.Close();
                                throw new System.ArgumentException("Echec de chargement des données !  Consulter l'administrateur");
                            }

                        }

                        sqlQueryResult.Close();
                        con.Close();

                    }
            }

            return items;
        }

        private CampaignFile MapItem(SqlDataReader sqlDataReader)
        {
            return new CampaignFile()
            {
                CampaignFileId = sqlDataReader.GetInt32(0),
                CampaignFileName = sqlDataReader.GetString(1)
            };
        }

    }
}