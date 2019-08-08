using System;
using Cima.Models.Shared;
using Cima.Models.TestModel;
using Cima.Repository.Shared;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;
using System.Globalization;

namespace Cima.Repository.TestData
{
    public class _REPO_EffectifCatSocioPro : BaseRepository<Effectif>
    {
        /// <summary>
        ///level est un indicateur du niveau :  année semestre ou trimestre
        /// </summary>
        private string level;

        /// <summary>
        ///GetHierachieUtilisable recupère la hierarchie à utiliser en réponse au filtre
        /// </summary>
        /// <param name="filtre"></param>
        /// <returns></returns>
        /// 
        private string GetHierachieUtilisable(FiltreDashboard filtre)
        {
            string hieraBloc = String.Empty;

            level = String.Empty;
            string theYear = String.Empty;
            string semester = String.Empty;
            string trimestre = String.Empty;
            string mois = String.Empty;

            hieraBloc = "[Temps].[Hierarchie Temps].";

            theYear = DateTime.Now.Year.ToString(); // recupère l'année courante

            // gestion semestre : chargement des variables de la hierarchie
            if (filtre.getAllFiltres().ContainsKey("semestre"))
            {
                level = "semester";
                theYear = filtre.DicoFiltres["annee"].Valeur;
                semester = filtre.DicoFiltres["semestre"].Valeur;
                hieraBloc += "[Calendar Semester].&[" + theYear + "]&[" + semester + "]";
            }

            //gestion trimestre : chargement des variables de la hierarchie
            else if (filtre.getAllFiltres().ContainsKey("trimestre"))
            {
                level = "quarter";
                theYear = filtre.DicoFiltres["annee"].Valeur;
                trimestre = filtre.DicoFiltres["trimestre"].Valeur;
                hieraBloc += "[Calendar Quarter].&[" + theYear + "]&[" + trimestre + "]";
            }

            //gestion année globale : chargement des variables de la hierarchie
            else
            {
                level = "mois";
                theYear = filtre.DicoFiltres["annee"].Valeur;
                mois = filtre.DicoFiltres["mois"].Valeur;
                hieraBloc += "[English Month Name].&[" + theYear + "]" + mois;
            }

            return hieraBloc;
        }

        public string buildQuerys(FiltreDashboard filtre)
        {

            //string annee = filtre.DicoFiltres["annee"].Valeur;

            string query = "WITH " +
                 "SET LastInsertDate AS Tail(FILTER([Temps].[Date Key].[Date Key]*" + GetHierachieUtilisable(filtre) + ", [Measures].[NombreEmployes]), 1) " +

                                "MEMBER [Measures].[NbreEmployes] AS SUM(LastInsertDate, [Measures].[NombreEmployes]) " +



                "select " +
                            "{[Measures].[NbreEmployes]} ON 0, " +
                            "([CategorieSocioPro].[LibCategorieSocioPro].[LibCategorieSocioPro]) on 1 " +
                           " FROM [SBI_Cube_Paie]"
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

        //Convertion de la CatégorieSocioPro en plage de salaire
        public string Convertir(string catsociopro)
        {
            //string catsociopro = ADR.GetString(1).ToString();
            switch (catsociopro)
            {
                //case "Ouvrier": { return "0 -100000"; }
                //case "Agent": { return "100000-150000"; }
                //case "Agent_maitrise": { return "75000 -100000"; }
                //case "Cadre": { return "200000 -250000"; }
                //case "Cadre_moyen": { return "250000 -300000"; }
                //case "Cadre_superieur": { return "300000 -350000 "; }
                //case "Employe_bureau": { return "0 -75000"; }
                //case "Manoeuvre": { return "0 -50000"; }
                //case "Technicien": { return "50000 -800000"; }
                //case "Technicien_superieur": { return "50000 -100000"; }
                //case "UNKNOW": { return "000 -00000"; }
                //default: { return ""; }

                case "Ouvrier": { return "Ouvrier"; }
                case "Agent": { return "Agent"; }
                case "Agent_Maitrise": { return "Agent maitrise"; }
                case "Cadre": { return "Cadre"; }
                case "Cadre_moyen": { return "Cadre moyen"; }
                case "Cadre_Superieur": { return "Cadre superieur"; }
                case "Employe_bureau": { return "Employe bureau"; }
                case "Manoeuvre": { return "Manoeuvre"; }
                case "Technicien": { return "Technicien"; }
                case "Technicien_superieur": { return "Techicien supérieur"; }
                case "UNKNOW": { return "UNKNOW"; }
                default: { return "UNKNOW"; }

            }

        }


        //Mapper les collones de la requête avec les attributs de l'objet
        protected override Effectif MapItem(AdomdDataReader reader)
        {
            Int16 vda;
            if (reader.IsDBNull(1)) vda = 0; else vda = reader.GetInt16(1);
            return new Effectif
            {
                //Xvalue = reader.GetString(1),
                CatSocioPro = Convertir(reader.GetString(0)),
                NbreEmploye = vda
            };
        }

        protected override ObservableCollection<Effectif> GetEmptyResult()
        {
            return new ObservableCollection<Effectif>();
        }


        //Lecture des résultas
        public ObservableCollection<Effectif> GetNumberofEmployeesbySalaryData(FiltreDashboard filtre)
        {
            ObservableCollection<Effectif> businessModelElts = new ObservableCollection<Effectif>();

            businessModelElts = this.Select(CONNECTION_STRING__GRH, filtre);

            return businessModelElts;
        }

    }
}