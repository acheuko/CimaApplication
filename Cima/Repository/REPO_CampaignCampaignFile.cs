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
    public class REPO_CampaignCampaignFile : SqlBaseRepository<CampaignCampaignFile>
    {

        public REPO_CampaignCampaignFile()
        {

        }

        public REPO_CampaignCampaignFile(SysmanDbContext context) : base(context)
        {

        }

        public ObservableCollection<CampaignFile> GetFilesByCampaignId(int IdCampaign)
        {
            ObservableCollection<CampaignFile> listcampaignFiles = new ObservableCollection<CampaignFile>();
            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT f.ID_filemask,libelle FROM sysman.tblFileMask f join sysman.tblCampaignFileMask c on f.ID_Filemask = c.ID_filemask WHERE c.ID_campaign = @paramId", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@paramId", IdCampaign);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {

                            CampaignFile file = new CampaignFile()
                            {
                                CampaignFileId = sqlQueryResult.GetInt32(0),
                                CampaignFileName = sqlQueryResult.GetString(1)
                            };

                            listcampaignFiles.Add(file);
                        }

                    }
            }

            return listcampaignFiles;
        }

        public int DeleteByIdCampaign(int IdCampagne)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = @"DELETE FROM sysman.tblCampaignFileMask WHERE ID_Campaign = @IdCampagne";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@IdCampagne", IdCampagne);

            int response = 0;

            try
            {
                response = cmd.ExecuteNonQuery();
                Console.WriteLine("Records deleted Successfully");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return response;
        }
    }
}