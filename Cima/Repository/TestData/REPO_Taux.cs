using System;
using Cima.Models.Shared;
using Cima.Models.TestModel;
using Cima.Repository.Shared;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;

namespace Cima.Repository.TestData
{
    public class REPO_Taux:BaseRepository<Taux>
    {
        private string annee;
        /// <summary>
        ///  GetHierachieTps : recupère la hierarchie à utiliser en réponse au filtre
        /// </summary>
        /// <param name="filtre"></param>
        /// <returns></returns>
        private string[] GetHierachieTps(FiltreDashboard filtre)
        {
            string annee = filtre.DicoFiltres["annee"].Valeur;
            string periodeEnCours = String.Empty;
            string periodePrecedente = String.Empty;
            string semester = String.Empty;
            string quater = String.Empty;
            string month = String.Empty;
            string[] result = new string[2];

            if (filtre.getAllFiltres().ContainsKey("semestre"))
            {

                // LastPeriods(4, [Temps].[Calendar Semester].&[2015]&[1])
                semester = filtre.DicoFiltres["semestre"].Valeur;

                periodeEnCours = "[Temps].[Calendar Semester].&[" + annee + "]&[" + semester + "]";
                periodePrecedente = "PARALLELPERIOD([Temps].[Calendar Semester].[Calendar Semester], 1 ," + periodeEnCours + ")";
                result[0] = periodeEnCours;
                result[1] = periodePrecedente;
            }

            else if (filtre.getAllFiltres().ContainsKey("trimestre"))
            {
                quater = filtre.DicoFiltres["trimestre"].Valeur;

                periodeEnCours = "[Temps].[Calendar Quarter].&[" + annee + "]&[" + quater + "]";
                periodePrecedente = "PARALLELPERIOD([Temps].[Calendar Quarter].[Calendar Quarter], 1 ," + periodeEnCours + ")";
                result[0] = periodeEnCours;
                result[1] = periodePrecedente;
            }

            else if (filtre.getAllFiltres().ContainsKey("mois"))
            {
                //if (filtre.DicoFiltres["mois"].Valeur == String.Empty)
                //{
                //    annee = year.ToString();
                //    month = "&[" + semestre + "]&[" + trimestre + "]&[" + moisName + "]";
                //}
                //else 
                month = filtre.DicoFiltres["mois"].Valeur;
                //[Temps].[Hierarchie Temps].[English Month Name].&[2015]&[2]&[3]&[July]


                periodeEnCours = "[Temps].[Hierarchie Temps].[English Month Name].&[" + annee + "]" + month;
                periodePrecedente = "PARALLELPERIOD([Temps].[Hierarchie Temps].[English Month Name], 1 ," + periodeEnCours + ")";
                result[0] = periodeEnCours;
                result[1] = periodePrecedente;
            }

            return result;
        }


        //Construire la requête
        public string buildQuerys(FiltreDashboard filtre)
        {



            string query = "WITH SET LastInsertDate AS TAIL(FILTER([Temps].[Date Key].[Date Key]*" + GetHierachieTps(filtre)[0] + ", [Measures].[NombreEmployes]), 1) " +
                           " MEMBER [Measures].[NbreEmp] as SUM (LastInsertDate, [Measures].[NombreEmployes])" +
                          "SET LastInsertDatePrec AS TAIL(FILTER([Temps].[Date Key].[Date Key]*" + GetHierachieTps(filtre)[1] + ", [Measures].[NombreEmployes]), 1) " +
                           " MEMBER [Measures].[NbreEmpPrec] as SUM (LastInsertDatePrec, [Measures].[NombreEmployes])" +
                " member TauxAnneeSelection as IIF([Measures].[NbreEmp]=0 OR isempty([Measures].[NbreEmp]),0," +
                           " [Measures].[EffectifDeparts]*100/[Measures].[NbreEmp])" +
                 " member TauxAnneePass as IIF([Measures].[NbreEmpPrec]=0 OR isempty([Measures].[NbreEmpPrec]),0," +
                           " [Measures].[EffectifDeparts]*100/[Measures].[NbreEmpPrec])" +
                           "member evolution as TauxAnneeSelection-TauxAnneePass" +
                           "  select" +
                           " {TauxAnneeSelection, evolution, TauxAnneePass} on 0" +
                          "   from [SBI_Cube_GPEC]";

            return query;

        }


        /// <summary>
        /// Execute la requête MDX recupérant les taux de recrutement
        /// </summary>
        /// <param name="filter"> filtre envoyé </param>
        /// <param name="bloc"></param>
        /// <returns> retourne le resultat de la requete </returns>
        protected override IDbCommand GetCommand(FiltreDashboard filter, string bloc=" ")
        {
            AdomdCommand command = new AdomdCommand(buildQuerys(filter));
            command.Connection = connect(CONNECTION_STRING_RASCOM);
            return command;
        }


        /// <summary>
        /// Mapper les colonnes de la requête avec les attributs de l'Objet 
        /// </summary>
        /// <param name="reader"> Ensemble des données de la requête </param>
        /// <returns> Retourne l'objet mappé </returns>
        protected override Taux MapItem(AdomdDataReader reader)
        {
            Double ty, ty1;
            if (reader.IsDBNull(0)) ty = 0; else ty = reader.GetDouble(0);


            if (reader.IsDBNull(1) || reader.GetDouble(1).ToString().Equals("-Infini")) ty1 = -100; else ty1 = reader.GetDouble(1);
            return new Taux
            {
                TauxY = ty,
                Tauy1 = ty1

            };
        }

        protected override ObservableCollection<Taux> GetEmptyResult()
        {
            return new ObservableCollection<Taux>();
        }


        /// <summary>
        /// construit l'ensemble d'Objets contenant les resultats de la requête.
        /// (une ligne d'enregistrement de la requête correspond à un Objet)
        /// </summary>
        /// <param name="filtre"> filtre envoyé </param>
        /// <param name="bloc"></param>
        /// <returns> Retourne l'ensemble d'objet </returns>
        public ObservableCollection<Taux> GetTauxRecrutementData(FiltreDashboard filtre)
        {
            ObservableCollection<Taux> OverviewRecrutementElts = new ObservableCollection<Taux>();

            OverviewRecrutementElts = this.Select(CONNECTION_STRING_RASCOM, filtre);

            return OverviewRecrutementElts;
        }

    }
}