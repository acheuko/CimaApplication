using Cima.Models.Shared;
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
    public class REPO_FiltreCriteria : SqlRepository<FiltreCriteria>
    {
        
        public string BuildFiltreCriteriaQuery(string bloc = "")
        {
            string query = String.Empty;
            switch (bloc)
            {
                case ANNEE: //Annee
                    {
                        query = @"SELECT distinct CalendarYear FROM dbo.tblTemps order by 1";
                        break;
                    }                
                case ZONE_ECONOMIQUE: //ZoneEconomique
                    {
                        query = @"SELECT ID_ZoneEconomique, ZoneEconomique_ID FROM dbo.tblZoneEconomique";
                        break;
                    }
                case CATEGORIE: //Categorie
                    {
                        query = @"SELECT ID_Categorie ,Categorie_ID FROM dbo.tblCategorie";
                        break;
                    }
                case PAYS: //Pays
                    {
                        query = @"SELECT ID_Pays, LibPays, ID_ZoneEconomique FROM dbo.tblPays";
                        break;
                    }
                case SOUS_CATEGORIE: //SousCategorie
                    {
                        query = @"SELECT ID_SousCategorie ,SousCategorie_ID, ID_Categorie FROM dbo.tblSousCategorie";
                        break;
                    }
                case SPECIALISATION: //Specialisation
                    {
                        query = @"SELECT ID_Specialisation, LibSpecialisation FROM dbo.tblSpecialisation";
                        break;
                    }
                case TYPE_ASSURANCE_VIE: //type Assurance vie
                    {
                        query = @"SELECT ID_TypeAssuranceVie,LibTypeAssuranceVie FROM dbo.tblTypeAssuranceVie";
                        break;
                    }
        
                case TYPE_ENTREPRISE: //TypeEntreprise
                    {
                        query = @"SELECT ID_TypeEntreprise, LibTypeEntreprise FROM dbo.tblTypeEntreprise";
                        break;
                    }
                case TYPE_REASSURANCE: //Type Reassurance
                    {
                        query = @"SELECT ID_TypeReassurance ,LibTypeReassurance FROM dbo.tblTypeReassurance";
                        break;
                    }
            }

            return query;
        }

        protected override FiltreCriteria MapItem(SqlDataReader reader)
        {
            return new FiltreCriteria()
            {
                ValueField = reader.GetInt16(0),
                TextField = reader.GetString(1)
            };
        }

        private FiltreCriteria MapItem(SqlDataReader reader, string filtreCriteriaName)
        {
            FiltreCriteria filtreCriteria = new FiltreCriteria();

            switch (filtreCriteriaName)
            {
                case ANNEE:
                    {
                        filtreCriteria.ValueField = reader.GetInt16(0);
                        break;
                    }
                case PAYS:
                    {
                        filtreCriteria.ValueField = reader.GetInt16(0);
                        filtreCriteria.TextField = reader.GetString(1);
                        filtreCriteria.ParentField = reader.GetByte(2);
                   
                        break;
                    }
                case SOUS_CATEGORIE:
                    {
                        filtreCriteria.ValueField = reader.GetInt16(0);
                        filtreCriteria.TextField = reader.GetString(1);
                        filtreCriteria.ParentField = reader.GetInt16(2);

                        break;
                    }
            }


            return filtreCriteria;
        }

        public ObservableCollection<FiltreCriteria> GetFiltreCriteriaByName(string filtreCriteriaName)
        {
            ObservableCollection<FiltreCriteria> items = new ObservableCollection<FiltreCriteria>();
            string sqlQuery = BuildFiltreCriteriaQuery(filtreCriteriaName);
            using (var sqlConnection = Connect(CONNECTION_STRING_DWH))
            using (var sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
            {
                using (var sqlQueryResult = sqlCommand.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            items.Add(MapItem(sqlQueryResult,filtreCriteriaName));
                        }

                    }
            }

            return items;

        }


        public Dictionary<string, ObservableCollection<FiltreCriteria>> GetFiltreCriteriaData()
        {
           
            Dictionary<string, ObservableCollection<FiltreCriteria>> dicoFiltre = new Dictionary<string, ObservableCollection<FiltreCriteria>>
            {
                { "zoneEconomique", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(ZONE_ECONOMIQUE)) },
                { "categorie", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(CATEGORIE)) },
                { "specialisation", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(SPECIALISATION)) },
                { "typeAssuranceVie", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(TYPE_ASSURANCE_VIE)) },
                { "typeEntreprise", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(TYPE_ENTREPRISE)) },
                { "typeReassurance", Select(CONNECTION_STRING_DWH, BuildFiltreCriteriaQuery(TYPE_REASSURANCE)) },
                { "annee", GetFiltreCriteriaByName("annee") }
            };

            return dicoFiltre;
        }
    }
}