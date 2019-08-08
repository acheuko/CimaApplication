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
    public class _REPO_SalaireEntiteAdmin : BaseRepository<Montant>
    {
        public string buildQuerys(FiltreDashboard filtre)
        {

            Dictionary<string, FiltreElement> dico = filtre.getAllFiltres();


            string[] entiteAdminItems;
            string libEntiteAdmin = String.Empty;

            string RmUnknow = ".CurrentMember.Name<>'UNKNOWN')";

            //Type Occupant
            if (filtre.getAllFiltres().ContainsKey("entiteAdmin"))
            {
                entiteAdminItems = dico["entiteAdmin"].Valeur.Split(FILTER_SEPARATOR);
                if (entiteAdminItems.Length == 0)
                {
                    libEntiteAdmin = "TOPCOUNT(filter([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],[EntiteAdmin].[LibEntiteAdmin]" + RmUnknow + ",6, [Measures].[SalaireNet]),";
                }
                else
                {
                    if (entiteAdminItems.Length == 1 && entiteAdminItems[0] == String.Empty) libEntiteAdmin = "TOPCOUNT(filter([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],[EntiteAdmin].[LibEntiteAdmin]" + RmUnknow + ",6, [Measures].[SalaireNet]),";

                    else
                    {
                        foreach (string str in entiteAdminItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libEntiteAdmin += "[EntiteAdmin].[LibEntiteAdmin].&[" + str + "],";
                        }

                    }

                }

            }

            string query = "select " +
                            "{[Measures].[SalaireBrut],[Measures].[Montant Prime]} ON 0, " +
                             "{" + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1) + "} on 1 " +
                //"TOPCOUNT(FILTER([EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],[EntiteAdmin].[LibEntiteAdmin].CURRENTMEMBER.NAME <> 'UNKNOWN'), 10,[Measures].[SalaireNet] ) ON 1 "

                           " FROM  [SBI_Cube_Paie] "
                           + buildGRHPaieWhereCondition(filtre, "type3");
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
            double vda;
            if (reader.IsDBNull(1)) vda = 0; else vda = reader.GetDouble(1);

            double vda1;
            if (reader.IsDBNull(2)) vda1 = 0; else vda1 = reader.GetDouble(2);

            return new Montant
            {
                EntiteAdmin = reader.GetString(0),
                MontantSalaire = vda / 1000000,
                Prime = vda1 / 1000000
            };
        }

        protected override ObservableCollection<Montant> GetEmptyResult()
        {
            return new ObservableCollection<Montant>();
        }

        //Lecture des résultas
        public ObservableCollection<Montant> GetPayrollBreakDownData(FiltreDashboard filtre)
        {
            ObservableCollection<Montant> businessModelElts = new ObservableCollection<Montant>();

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