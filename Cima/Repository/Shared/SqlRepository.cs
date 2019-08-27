using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cima.Repository.Shared
{
    public abstract class SqlRepository<T> : AbstractRepository
    {

        protected abstract T MapItem(SqlDataReader reader);

        protected override IDbCommand GetCommand(string query, IDbConnection connection)
        {
            SqlCommand sqlcommand = new SqlCommand(query)
            {
                Connection = (SqlConnection)connection
            };

            return sqlcommand;
        }

        protected SqlConnection Connect(string dsConnection)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[dsConnection].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw new System.ArgumentException("Echec de connexion à la BD, vérifier serveur de données !");
            }
        }

        public ObservableCollection<T> Select(string dsConnection, string query)
        {
            SqlConnection con = (SqlConnection)this.Connect(dsConnection);
            ObservableCollection<T> items = new ObservableCollection<T>();
            SqlCommand sqlCmd;
                        
            using (sqlCmd = (SqlCommand)GetCommand(query, con))
            {
                using (var sqlQueryResult = sqlCmd.ExecuteReader())
                    
                    if (sqlQueryResult != null && sqlQueryResult.HasRows)
                    {

                        while (sqlQueryResult.Read())
                        {
                            try
                            {
                                items.Add(MapItem(sqlQueryResult));
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sqlQueryResult.Close();
                                con.Close();
                                throw new System.ArgumentException("Echec de chargement des données !  Consulter l'administrateur");
                            }
                        }
                    }
                      
            }

            con.Close();

            return items;
        }
    }
}