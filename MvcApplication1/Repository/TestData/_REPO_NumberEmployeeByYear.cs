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
    public class _REPO_NumberEmployeeByYear : BaseRepository<Effectif>
    {
        /// <summary>
        ///  GetHierachieTps : recupère la hierarchie à utiliser en réponse au filtre
        /// </summary>
        /// <param name="filtre"></param>
        /// <returns></returns>
        private string GetHierachieTps(FiltreDashboard filtre)
        {
            string annee = filtre.DicoFiltres["annee"].Valeur;
            string periode = String.Empty;
            string semester = String.Empty;
            string quater = String.Empty;
            string month = String.Empty;
            string monthFullName = String.Empty;

            DateTime maintenant = DateTime.Now;
            int year = maintenant.Year;
            int semestre = (maintenant.Month - 1) / 6 + 1;
            int trimestre = (maintenant.Month - 1) / 3 + 1;
            int mois = maintenant.Month;

            string moisName = maintenant.ToString("MMMM", new CultureInfo("en-US"));

            if (filtre.getAllFiltres().ContainsKey("semestre"))
            {

                // LastPeriods(4, [Temps].[Calendar Semester].&[2015]&[1])
                semester = filtre.DicoFiltres["semestre"].Valeur;

                periode = "[Temps].[Calendar Semester].&[" + annee + "]&[" + semester + "]";


            }

            else if (filtre.getAllFiltres().ContainsKey("trimestre"))
            {
                quater = filtre.DicoFiltres["trimestre"].Valeur;

                periode = "[Temps].[Calendar Quarter].&[" + annee + "]&[" + quater + "]";

            }

            else if (filtre.getAllFiltres().ContainsKey("mois"))
            {
                month = filtre.DicoFiltres["mois"].Valeur;

                //[Temps].[Hierarchie Temps].[English Month Name].&[2015]&[2]&[3]&[July]


                periode = "[Temps].[Hierarchie Temps].[English Month Name].&[" + annee + "]" + month;

            }
            else
            {
                periode = "[Temps].[Hierarchie Temps].[English Month Name].&[" + year + "]&[" + semestre + "]&[" + trimestre + "]&[" + moisName + "]";
            }

            return periode;
        }

        public string buildQuerys(FiltreDashboard filtre)
        {
            Dictionary<string, FiltreElement> dico = filtre.getAllFiltres();
            string libAnnee = String.Empty;
            string annee = dico["annee"].Valeur;

            if (annee == null || annee == String.Empty) libAnnee = "[Temps].[Calendar Year].&[" + currentYear() + "]";
            else libAnnee = "[Temps].[Calendar Year].&[" + annee + "]";

            string query =
                "with member EffTemp as [Measures].[NombreSalaries]-[Measures].[Effectif Personnel CDI]" +
                "select " +
                           "{ [Measures].[Effectif Personnel CDI],EffTemp,[Measures].[NombreSalaries] } ON COLUMNS," +
                           "{[Temps].[Calendar Year].&[" + annee + "]*[Temps].[English Month Name].[English Month Name]}  ON ROWS " +
                          " from [SBI_Cube_Paie] "
                           + buildGRHPaieWhereCondition(filtre, "type1")
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
        protected override Effectif MapItem(AdomdDataReader reader)
        {
            Int32 nbEmploye;
            Int32 nbTemporaire;
            Int32 nbEmployePermanent;
            if (reader.IsDBNull(2)) nbEmployePermanent = 0; else nbEmployePermanent = reader.GetInt32(2);
            if (reader.IsDBNull(3)) nbTemporaire = 0; else nbTemporaire = reader.GetInt32(3);
            if (reader.IsDBNull(4)) nbEmploye = 0; else nbEmploye = reader.GetInt32(4);
            // int l = reader.GetString(3).ToString().Length;

            return new Effectif
            {
                MoisSubstring = reader.GetString(1).ToString().Substring(0, 3),
                Mois = reader.GetString(1).ToString(),
                NbreEmployePermanent = nbEmployePermanent,
                NbreEmployeTemporaire = nbTemporaire,
                NbreEmploye = nbEmploye

            };
        }

        protected override ObservableCollection<Effectif> GetEmptyResult()
        {
            return new ObservableCollection<Effectif>();
        }


        //Lecture des résultas
        public ObservableCollection<Effectif> GetNumberofEmployeesbyYearData(FiltreDashboard filtre)
        {
            ObservableCollection<Effectif> businessModelElts = new ObservableCollection<Effectif>();

            businessModelElts = this.Select(CONNECTION_STRING__GRH, filtre);

            return businessModelElts;
        }
    }
}