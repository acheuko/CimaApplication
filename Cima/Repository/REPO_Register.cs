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
    public class REPO_Register : AbstractRepository 
    {
        public int RegisterUser(User user)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = "INSERT INTO sysman.tblUser (Login, Password, Salt, FirstName, LastName,Company, Profils) VALUES (@Login,@Password,@Salt, @FirstName, @LastName,@Company, @Profil)";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@Login", user.Login);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Company", user.Company);
            cmd.Parameters.AddWithValue("@Profil", user.Profils);

            int response = 0;

            try
            {
                response = cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
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

        public ObservableCollection<User> GetUserByUsername(string username)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<User> items = new ObservableCollection<User>();


            using (var sqlQuery = new SqlCommand(@"SELECT Login, Password, Profils, Company, Salt FROM sysman.tblUser WHERE Login = @Username", con))
            {

      
                sqlQuery.Parameters.AddWithValue("@Username", username);
              
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {

                        while (sqlQueryResult.Read())
                        {
                            try
                            {
                                items.Add(new User()
                                {
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