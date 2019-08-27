using Cima.Helpers;
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
    public class REPO_Login:SqlRepository<LoginModel>
    {
        public ObservableCollection<User> GetUserByLoginAndPassword(LoginModel u, string criteria)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<User> items = new ObservableCollection<User>();

            string query;
            if (CriteriaHelper.LOGIN.Equals(criteria))
                query = @"SELECT Login, Password, Profils, Company, Salt FROM sysman.tblUser WHERE Login = @Login";
            else if (CriteriaHelper.PASSWORD.Equals(criteria))
                query = @"SELECT Login, Password, Profils, Company, Salt FROM sysman.tblUser WHERE Password = @Password";
            else
                throw new Exception("The query string to execute is empty");

            using (var sqlQuery = (SqlCommand) GetCommand(query, con))
            {
               
                if (CriteriaHelper.LOGIN.Equals(criteria))
                    sqlQuery.Parameters.AddWithValue("@Login", u.UserName);
                else if (CriteriaHelper.PASSWORD.Equals(criteria))
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
                                                        Company = sqlQueryResult.GetString(3),
                                                        Salt = sqlQueryResult.GetString(4)
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

        protected override LoginModel MapItem(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}