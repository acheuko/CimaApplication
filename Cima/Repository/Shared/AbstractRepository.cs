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
        protected const string CONNECTION_STRING_DWH = "DWH_DS_CONNECTION_STRING";
        #endregion

        #region Criteria Name 
        protected const string ANNEE = "annee";
        protected const string ZONE_ECONOMIQUE = "zoneEconomique";
        protected const string CATEGORIE = "categorie";
        protected const string PAYS = "pays";
        protected const string SOUS_CATEGORIE = "sousCategorie";
        protected const string SPECIALISATION = "specialisation";
        protected const string TYPE_ASSURANCE_VIE = "typeAssuranceVie";
        protected const string TYPE_ENTREPRISE = "typeEntreprise";
        protected const string TYPE_REASSURANCE = "typeReassurance";
        #endregion


        protected abstract IDbCommand GetCommand(string query, IDbConnection connection);

        //protected abstract SqlConnection Connect(string dsConnection);

       
    }
}