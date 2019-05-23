using System;
using MvcApplication1.Models.Shared;
using MvcApplication1.Models.TestModel;
using MvcApplication1.Repository.Shared;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;
using System.Globalization;

namespace MvcApplication1.Repository.TestData
{
    public class _REPO_TaxDeductionGraph : BaseRepository<Montant>
    {
        public string buildQuerys(FiltreDashboard filtre)
        {




            string query = "select " +
                            "{ [Measures].[MontantRetenue] } ON COLUMNS," +
                            " {[Temps].[English Month Name].[English Month Name]} ON rows " +
                            " FROM [SBI_Cube_Paie] "
                             + buildGRHPaieWhereCondition(filtre, "type2")
                            ;


            return query;

        }

        //Exécuter la requête
        protected override IDbCommand GetCommand(FiltreDashboard filter, string bloc)
        {
            AdomdCommand command = new AdomdCommand(buildQuerys(filter));
            command.Connection = connect(CONNECTION_STRING__GRH);
            return command;
        }

        //Mapper les collones de la requête avec les attributs de l'objet
        protected override Montant MapItem(AdomdDataReader reader)
        {
            Double vda;
            if (reader.IsDBNull(1)) vda = 0; else vda = reader.GetDouble(1);

            return new Montant
            {

                MoisSubstring = reader.GetString(0).Substring(0, 3),
                Mois = reader.GetString(0).ToString(),
                MontantDeduction = vda / 1000000

            };
        }

        protected override ObservableCollection<Montant> GetEmptyResult()
        {
            return new ObservableCollection<Montant>();
        }

        //Lecture des résultas
        public ObservableCollection<Montant> GetTaxDeductionGraphData(FiltreDashboard filtre)
        {
            ObservableCollection<Montant> businessModelElts = new ObservableCollection<Montant>();

            businessModelElts = this.Select(CONNECTION_STRING__GRH, filtre);

            return businessModelElts;
        }
    }
}