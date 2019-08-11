using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cima.Repository.Shared
{
    public abstract class AbstractRepository
    {
        #region Chaines de connexion
        protected const string CONNECTION_STRING_SYSMAN =  "SYSMAN_DS_CONNECTION";
        #endregion

        protected abstract IDbCommand GetCommand(string query, IDbConnection sqlconnection);
        

        protected SqlConnection connect(string dsConnection)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[dsConnection].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();
                return conn;
            }
            catch
            {

                throw new System.ArgumentException("Echec de connexion à la BD, vérifier serveur de données !");
            }
        }
    }
}