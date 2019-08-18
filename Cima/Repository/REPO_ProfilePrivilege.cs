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
    public class REPO_ProfilePrivilege : AbstractRepository
    {
        protected override IDbCommand GetCommand(string query, IDbConnection sqlconnection)
        {
            SqlCommand sqlcommand = new SqlCommand(query)
            {
                Connection = (SqlConnection)sqlconnection
            };

            return sqlcommand;
        }

        public List<Menu> GetMenuItems(string ProfileName)
        {
            List<Menu> MenuItems = new List<Menu>();

            string query = @"SELECT ProfilName, Name, Controller, [Action], ID_MenuItems, ID_parent, Icon " +
                            "FROM sysman.tblProfilUser  " +
                            "JOIN sysman.tblProfilPrivilege on ID_UserProfil = ID_Profil " +
                            "JOIN sysman.tblMenuItems  on ID_MenuItems = ID_Auto " +
                            "WHERE ProfilName = @ProfilName";

            using (var sqlConnection = this.connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(query, sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@ProfilName", ProfileName);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                { 
                    if (sqlQueryResult.HasRows)
                    {
                        while (sqlQueryResult.Read())
                        {

                            if (sqlQueryResult.IsDBNull(5))
                            {
                                int IdParent = sqlQueryResult.GetInt32(4);
                                List<Menu> childMenu = GetChildMenuItems(ProfileName,IdParent);

                                Menu menu = new Menu()
                                {
                                    Name = sqlQueryResult.GetString(1),
                                    Controller = sqlQueryResult.GetString(2),
                                    Action = sqlQueryResult.GetString(3),
                                    Icon = sqlQueryResult.GetString(6),
                                    MenuItems = childMenu
                                };

                                MenuItems.Add(menu);
                            }


                        }

                        sqlQueryResult.Close();

                    }
            }
            }

            return MenuItems;
        }


        private List<Menu> GetChildMenuItems(string ProfileName, int IdParent)
        {
            List<Menu> childMenu = new List<Menu>();
            

            string childquery = @"SELECT ProfilName, Name, Controller, [Action], ID_MenuItems, ID_Parent " +
                            "FROM sysman.tblProfilUser  " +
                            "JOIN [sysman].[tblProfilPrivilege]  on ID_UserProfil = ID_Profil " +
                            "JOIN sysman.tblMenuItems  on ID_MenuItems = [ID_Auto] " +
                            "WHERE ProfilName = @ProfilName AND ID_Parent = @IdParent";


            using (var sqlConnection = this.connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(childquery, sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@ProfilName", ProfileName);
                sqlQuery.Parameters.AddWithValue("@IdParent", IdParent);
                using (var sqlCommandResult = sqlQuery.ExecuteReader())
                    if (sqlCommandResult.HasRows)
                    {
                        while (sqlCommandResult.Read())
                        {
                            Menu childMenuItems = new Menu()
                            {
                                Name = sqlCommandResult.GetString(1),
                                Controller = sqlCommandResult.GetString(2),
                                Action = sqlCommandResult.GetString(3)
                                
                            };

                            childMenu.Add(childMenuItems);
                        }

                        sqlCommandResult.Close();

                    }
            }

            return childMenu;
        }
    }
}