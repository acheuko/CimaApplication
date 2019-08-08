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
    public class _REPO_TopFiveEntiteAdmin : BaseRepository<Effectif>
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
            Dictionary<string, FiltreElement> dico = filtre.getAllFiltres();


            string[] entiteAdminItems;
            string libEntiteAdmin = String.Empty;
            string annee = filtre.DicoFiltres["annee"].Valeur;

            string RmUnknow = ".CurrentMember.Name<>'UNKNOWN')";

            ////Entite admin
            //if (filtre.getAllFiltres().ContainsKey("entiteAdmin"))
            //{
            //    entiteAdminItems = dico["entiteAdmin"].Valeur.Split(FILTER_SEPARATOR);
            //    if (entiteAdminItems.Length == 0)
            //    {
            //        libEntiteAdmin = "TOPCOUNT([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],6, [Measures].[NombreEmployes]),";
            //    }
            //    else
            //    {
            //        if (entiteAdminItems.Length == 1 && entiteAdminItems[0] == String.Empty) libEntiteAdmin = "TOPCOUNT([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],6, [Measures].[NombreEmployes]),";

            //        else
            //        {
            //            foreach (string str in entiteAdminItems)
            //            {
            //                //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
            //                libEntiteAdmin += "[EntiteAdmin].[LibEntiteAdmin].&[" + str + "],";
            //            }

            //        }

            //    }

            //}

            //Entite admin
            if (filtre.getAllFiltres().ContainsKey("entiteAdmin"))
            {
                entiteAdminItems = dico["entiteAdmin"].Valeur.Split(FILTER_SEPARATOR);
                if (entiteAdminItems.Length == 0)
                {
                    libEntiteAdmin = "TOPCOUNT([EntiteAdmin].[EntiteAdminParent].LEVELS(1),5, [Measures].[NombreEmployes]),";
                }
                else
                {
                    if (entiteAdminItems.Length == 1 && entiteAdminItems[0] == String.Empty) libEntiteAdmin = "TOPCOUNT([EntiteAdmin].[EntiteAdminParent].LEVELS(1),5, [Measures].[NombreEmployes]),";

                    else
                    {
                        foreach (string str in entiteAdminItems)
                        {
                            //[EntiteAdmin].[EntiteAdminParent].[Conseil administration]
                            libEntiteAdmin += "TOPCOUNT([EntiteAdmin].[EntiteAdminParent].[" + str + "].CHILDREN,5, [Measures].[NombreEmployes]),";
                        }

                    }

                }

            }

            //string query = " with member Effectif" +
            //                " as ([Measures].[NombreEmployes],[EntiteAdmin].[LibEntiteAdmin]).Count" +
            //                " select {Effectif} on 0" +
            //               ",{[EntiteAdmin].[LibEntiteAdmin].&[Direction des Affaires Générales],[EntiteAdmin].[LibEntiteAdmin].&[Direction des Affaires Législatives et Réglementaires]"+
            //               " ,[EntiteAdmin].[LibEntiteAdmin].&[Direction des Projets],[EntiteAdmin].[LibEntiteAdmin].&[Direction des Systèmes d'Informations], [EntiteAdmin].[LibEntiteAdmin].&[Direction du Budget],"+
            //               "[EntiteAdmin].[LibEntiteAdmin].&[Division Administrative et Financier],[EntiteAdmin].[LibEntiteAdmin].&[Division Commerciale],"+
            //               " [EntiteAdmin].[LibEntiteAdmin].&[Division des Etudes et des Affaires Juridiques],[EntiteAdmin].[LibEntiteAdmin].&[Division Marking]}on 1"+
            //               "from [SBI_Cube_Paie]"
            //                + buildGRHPaieWhereCondition(filtre, "type3");

            string query =

               "WITH " +
               "SET LastInsertDate AS Tail(FILTER([Temps].[Date Key].[Date Key]*" + GetHierachieUtilisable(filtre) + ", [Measures].[NombreEmployes]), 1) " +

                              "MEMBER [Measures].[NbreEmployes] AS SUM(LastInsertDate, [Measures].[NombreEmployes]) " +



                "SELECT [Measures].[NbreEmployes] ON 0, " +
                        "TOPCOUNT([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],5, [Measures].[NombreEmployes]) on 1 "+
                         // "{" + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1) + "} on 1 " +
                          "FROM [SBI_Cube_Paie] "
                            + buildGRHPaieWhereCondition(filtre, "type15")
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
            Int32 nbreEmploye = 0;
            string entiteAdminParent = string.Empty;


            for (int i = 2; i < 10; i++)
            {
                if (reader.FieldCount == i)
                {
                    if (reader.IsDBNull(i - 1)) nbreEmploye = 0; else nbreEmploye = reader.GetInt32(i - 1);
                    entiteAdminParent = reader.GetString(i - 2);
                }
            }

            //if (reader.FieldCount == 2)
            //{
            //    if (reader.IsDBNull(1)) nbreEmploye = 0; else nbreEmploye = reader.GetInt32(1);
            //    entiteAdminParent = reader.GetString(0);
            //}

            //if (reader.FieldCount == 3)
            //{
            //    if (reader.IsDBNull(2)) nbreEmploye = 0; else nbreEmploye = reader.GetInt32(2);
            //    entiteAdminParent = reader.GetString(1);
            //}

            //if (reader.FieldCount == 4)
            //{
            //    if (reader.IsDBNull(3)) nbreEmploye = 0; else nbreEmploye = reader.GetInt32(3);
            //    entiteAdminParent = reader.GetString(2);
            //}

            //if (reader.FieldCount == 5)
            //{
            //    if (reader.IsDBNull(4)) nbreEmploye = 0; else nbreEmploye = reader.GetInt32(4);
            //    entiteAdminParent = reader.GetString(3);
            //}

            return new Effectif
            {
                EntiteAdmin = entiteAdminParent,
                NbreEmploye = nbreEmploye



            };
        }

        protected override ObservableCollection<Effectif> GetEmptyResult()
        {
            return new ObservableCollection<Effectif>();
        }

        //Lecture des résultas
        public ObservableCollection<Effectif> GetHeadCountData(FiltreDashboard filtre)
        {
            ObservableCollection<Effectif> businessModelElts = new ObservableCollection<Effectif>();

            businessModelElts = this.Select(CONNECTION_STRING__GRH, filtre);

            return businessModelElts;
        }

        public string gestiondescaracteres(string chaine)
        {
            if (chaine.Length >= 27)
            {
                return chaine.Substring(9, chaine.Length - 9);

            }
            return chaine;
        }
    }
}