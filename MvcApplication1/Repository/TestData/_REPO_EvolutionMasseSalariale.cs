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
    public class _REPO_EvolutionMasseSalariale : BaseRepository<Montant>
    {
        public string buildQuerys(FiltreDashboard filtre)
        {
            Dictionary<string, FiltreElement> dico = filtre.getAllFiltres();
            string libAnnee = String.Empty;
            string annee = dico["annee"].Valeur;

            if (annee == null || annee == String.Empty) libAnnee = "[Temps].[Calendar Year].&[" + currentYear() + "]";
            else libAnnee = "[Temps].[Calendar Year].&[" + annee + "]";


            //string query = "with member tauxTY as ([Measures].[Taux d’absentéisme], [Temps].[Calendar Year].&[" + annee + "]) " +
            //               "member tausPY as ([Measures].[Taux d’absentéisme],[Temps].[Calendar Year].&[" + annee + "].PREVMEMBER) " +
            //               "select {tauxTY,tausPY} on 0 " +
            //               ",{[Temps].[Calendar Year].&[" + annee + "]*[Temps].[English Month Name].[English Month Name]} on 1 " +
            //               "from [SBI_Cube_TempsConge] "
            //               + buildGRHPaieWhereCondition(filtre, "type21");



            string query = "with " +
                           "member masseSalariale as [Measures].[SalaireBrut] + [Measures].[ImpotPatronal]-[Measures].[FraisMission]-[Measures].[CHARGES DES BOURSIERS] " +
                           "member masseSalairEncour as (masseSalariale, [Temps].[Calendar Year].&[" + annee + "]) " +
                           "member masseSalairPrec as (masseSalariale,[Temps].[Calendar Year].&[" + annee + "].PREVMEMBER) " +
                           "member masseSalairPrec_1 as (masseSalariale,[Temps].[Calendar Year].&[" + annee + "].PREVMEMBER.PREVMEMBER) " +
                            "member masseSalairPrec_2 as (masseSalariale,[Temps].[Calendar Year].&[" + annee + "].PREVMEMBER.PREVMEMBER.PREVMEMBER) " +
                             "member masseSalairPrec_3 as (masseSalariale,[Temps].[Calendar Year].&[" + annee + "].PREVMEMBER.PREVMEMBER.PREVMEMBER.PREVMEMBER) " +
                           "select " +
                            " { [Measures].[masseSalairEncour],[Measures].[masseSalairPrec],masseSalairPrec_1,masseSalairPrec_2,masseSalairPrec_3} ON COLUMNS, " +
                           "{[Temps].[Calendar Year].&[" + annee + "]*[Temps].[English Month Name].[English Month Name]}  ON ROWS " +
                           "  FROM [SBI_Cube_Paie]"
                          + buildGRHPaieWhereCondition(filtre, "type1");

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
            Double ms, ms_1, ms_2, ms_3, ms_4;
            if (reader.IsDBNull(2)) ms = 0; else ms = reader.GetDouble(2);

            if (reader.IsDBNull(3)) ms_1 = 0; else ms_1 = reader.GetDouble(3);

            if (reader.IsDBNull(4)) ms_2 = 0; else ms_2 = reader.GetDouble(4);
            if (reader.IsDBNull(5)) ms_3 = 0; else ms_3 = reader.GetDouble(5);
            if (reader.IsDBNull(6)) ms_4 = 0; else ms_4 = reader.GetDouble(6);

            return new Montant
            {

                Mois = reader.GetString(1).Substring(0, 3),
                MontantSalaire = ms / 1000000,
                MontantSalaireN_1 = ms_1 / 1000000,
                MontantSalaireN_2 = ms_2 / 1000000,
                MontantSalaireN_3 = ms_3 / 1000000,
                MontantSalaireN_4 = ms_4 / 1000000

            };
        }

        protected override ObservableCollection<Montant> GetEmptyResult()
        {
            return new ObservableCollection<Montant>();
        }

        //Lecture des résultas
        public ObservableCollection<Montant> GetSalaryGraphData(FiltreDashboard filtre)
        {
            ObservableCollection<Montant> businessModelElts = new ObservableCollection<Montant>();

            businessModelElts = this.Select(CONNECTION_STRING__GRH, filtre);

            return businessModelElts;
        }
    }
}