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
    public class REPO_CampaignCampaignControl : SqlBaseRepository<CampaignCampaignControl>
    {
        public REPO_CampaignCampaignControl(SysmanDbContext context) : base(context)
        {

        }

        public REPO_CampaignCampaignControl()
        {

        }

        public ObservableCollection<string> GetByCampaignIdAndControlType(int IdCampagne, string ControlType)
        {
            ObservableCollection<string> result = new ObservableCollection<string>();

            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT Blocking FROM sysman.tblCampaignControl WHERE ID_Campaign = @ParamId AND ID_ControlType = @ParamControlType", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@ParamId", IdCampagne);
                sqlQuery.Parameters.AddWithValue("@ParamControlType", ControlType);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            var blocking = sqlQueryResult.GetString(0);

                            result.Add(blocking);
                        }

                    }
            }

            return result;
        }
    }
}