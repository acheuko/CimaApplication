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
    public class REPO_Login:AbstractRepository
    {
        public ObservableCollection<User> GetUserByLoginAndPassword(LoginModel u)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<User> items = new ObservableCollection<User>();
            using (var sqlQuery = new SqlCommand(@"SELECT Login, Password, Profils, Company FROM sysman.tblUser WHERE Login = @Login And Password = @Password", con))
            {
                sqlQuery.Parameters.AddWithValue("@Login", u.UserName);
                sqlQuery.Parameters.AddWithValue("@Password", u.Password);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {

                        while (sqlQueryResult.Read())
                        {
                            try
                            {
                                items.Add(new User() {
                                                        Login = sqlQueryResult.GetString(0),
                                                        Password = sqlQueryResult.GetString(1),
                                                        Profils = sqlQueryResult.GetString(2),
                                                        Company = sqlQueryResult.GetString(3)
                                                    }
                                          );
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

        protected override IDbCommand GetCommand(string query, IDbConnection sqlconnection)
        {
            SqlCommand sqlcommand = new SqlCommand(query)
            {
                Connection = (SqlConnection)sqlconnection
            };

            return sqlcommand;
        }

    }
}