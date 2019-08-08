using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;
using System.Configuration;
using Cima.Models.Shared;
using System.Globalization;


namespace Cima.Repository.Shared
{
    public abstract class BaseRepository<T>
    {
        #region Chaines de connexion
        protected const string CONNECTION_STRING_RASCOM = "RASCOM_DB_Connection";
        protected const string CONNECTION_STRING__GRH = "SBI_GRH_Connection";
        #endregion

        protected const char FILTER_SEPARATOR = ';';

        protected abstract ObservableCollection<T> GetEmptyResult();
        protected abstract IDbCommand GetCommand(FiltreDashboard filter ,string bloc=" ");
        protected abstract T MapItem(AdomdDataReader reader);

        protected AdomdConnection connect(string dsConnection)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings[dsConnection].ConnectionString;
                AdomdConnection conn = new AdomdConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch
            {

                throw new System.ArgumentException("Echec de connexion à la BD, vérifier serveur de données !");
            }
        }

        public ObservableCollection<T> Select(string dsConnection, FiltreDashboard filter, string bloc="")
        {
            if (filter == null) throw new ArgumentNullException();
            //try
            //{
            ObservableCollection<T> items = this.GetEmptyResult();
            AdomdCommand cmd;

            using (AdomdConnection con = this.connect(dsConnection))
            {
                cmd = (AdomdCommand)this.GetCommand(filter,bloc);
                AdomdDataReader objReader = cmd.ExecuteReader();
                while (objReader.Read())
                {
                    try
                    {
                    items.Add(this.MapItem(objReader));
                    }
                    catch
                    {
                        objReader.Close();
                        con.Close();
                        throw new System.ArgumentException("Echec de chargement des données !  Consulter l'administrateur");
                    }
                }
                objReader.Close();
                con.Close();
            }
            return items;
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message == "Echec de chargement des données !  Consulter l'administrateur") throw ex;
            //    else
            //        throw new System.ArgumentException("Echec d'exécution d'une requête ! Consulter l'administrateur");
            //}
        }

        public int currentYear()
        {
            DateTime maintenant = DateTime.Now;
            int annee = maintenant.Year;

            return annee;
        }

        #region Condition du 'where' des requetes des REPO de GRH & Paie

        public string buildGRHPaieWhereCondition(FiltreDashboard filtre, string typeOfCondition)
        {
            string whereCondition = String.Empty;
            Dictionary<string, FiltreElement> dico = filtre.getAllFiltres();

            string anneeItem = dico["annee"].Valeur;
            string[] entiteAdminItems;
            string[] trancheAgeItems;
            string[] trancheAncienneteItems;
            string sexeItems = dico["sexe"].Valeur;
            string[] typeContratItems;
            string[] emploiTypeItems;
            string[] sourceRecrutementItems;
            string[] typeFormationItems;
            string[] domaineItems;
            string[] organismeFormateurItems;
            string[] qualificationItems;
            string[] categoriesociItems;
            string[] situationmatItems;
            string[] recruteurItems;
            string[] typeAbsenceItems;
            string[] motifAbsenceItems;


            string libEntiteAdmin = String.Empty;
            string libTrancheAge = String.Empty;
            string libTrancheAnciennete = String.Empty;
            string libTypeFormation = string.Empty;
            string libDomaine = string.Empty;
            string libOrganismeFormateur = string.Empty;
            string libTypeContrat = String.Empty;
            string libEmploiType = String.Empty;
            string libSourceRecrutement = String.Empty;
            string libSexe = String.Empty;
            string libAnnee = String.Empty;
            string libQualification = String.Empty;
            string libCatsociopro = String.Empty;
            string libSituationMat = String.Empty;
            string libRecruteur = string.Empty;
            string libtypeAbsence = String.Empty;
            string libmotifAbsence = String.Empty;
            string annee = filtre.DicoFiltres["annee"].Valeur;

            if (filtre.getAllFiltres().ContainsKey("typeContrat"))
            {
                typeContratItems = dico["typeContrat"].Valeur.Split(FILTER_SEPARATOR);
                if (typeContratItems.Length == 0)
                {
                    libTypeContrat = "[TypeContrat].[LibTypeContrat].[LibTypeContrat],";
                }
                else
                {
                    if (typeContratItems.Length == 1 && typeContratItems[0] == String.Empty) libTypeContrat = "[TypeContrat].[LibTypeContrat].[LibTypeContrat],";

                    else
                    {
                        foreach (string str in typeContratItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libTypeContrat += "[TypeContrat].[LibTypeContrat].&[" + str + "],";
                        }

                    }

                }
            }

            if (filtre.getAllFiltres().ContainsKey("emploiType"))
            {
                emploiTypeItems = dico["emploiType"].Valeur.Split(FILTER_SEPARATOR);
                if (emploiTypeItems.Length == 0)
                {
                    libEmploiType = "[EmploiType].[LibEmploiType].[LibEmploiType],";
                }
                else
                {
                    if (emploiTypeItems.Length == 1 && emploiTypeItems[0] == String.Empty) libEmploiType = "[EmploiType].[LibEmploiType].[LibEmploiType],";

                    else
                    {
                        foreach (string str in emploiTypeItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libEmploiType += "[EmploiType].[LibEmploiType].&[" + str + "],";
                        }

                    }

                }

            }

            if (filtre.getAllFiltres().ContainsKey("sourceRecrutement"))
            {
                sourceRecrutementItems = dico["sourceRecrutement"].Valeur.Split(FILTER_SEPARATOR);
                if (sourceRecrutementItems.Length == 0)
                {
                    libSourceRecrutement = "[SourceRecrutement].[LibSourceRecrutement].[LibSourceRecrutement],";
                }
                else
                {
                    if (sourceRecrutementItems.Length == 1 && sourceRecrutementItems[0] == String.Empty) libSourceRecrutement = "[SourceRecrutement].[LibSourceRecrutement].[LibSourceRecrutement],";

                    else
                    {
                        foreach (string str in sourceRecrutementItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libSourceRecrutement += "[SourceRecrutement].[LibSourceRecrutement].&[" + str + "],";
                        }

                    }

                }

            }

            if (filtre.getAllFiltres().ContainsKey("typeFormation"))
            {
                typeFormationItems = dico["typeFormation"].Valeur.Split(FILTER_SEPARATOR);
                if (typeFormationItems.Length == 0)
                {
                    libTypeFormation = "[Type Formation].[LibelleTypeFormation].[LibelleTypeFormation],";
                }
                else
                {
                    if (typeFormationItems.Length == 1 && typeFormationItems[0] == String.Empty)
                        libTypeFormation = "[Type Formation].[LibelleTypeFormation].[LibelleTypeFormation],";

                    else
                    {
                        foreach (string str in typeFormationItems)
                        {
                            libTypeFormation += "[Type Formation].[LibelleTypeFormation].&[" + str + "],";
                        }

                    }

                }
            }

            if (filtre.getAllFiltres().ContainsKey("domaineFormation"))
            {
                domaineItems = dico["domaineFormation"].Valeur.Split(FILTER_SEPARATOR);
                if (domaineItems.Length == 0)
                {
                    libDomaine = "[CatalogueFormation].[DomaineFormation].[DomaineFormation],";
                }
                else
                {
                    if (domaineItems.Length == 1 && domaineItems[0] == String.Empty)
                        libDomaine = "[CatalogueFormation].[DomaineFormation].[DomaineFormation],";

                    else
                    {
                        foreach (string str in domaineItems)
                        {
                            libDomaine += "[CatalogueFormation].[DomaineFormation].&[" + str + "],";
                        }

                    }

                }
            }

            if (filtre.getAllFiltres().ContainsKey("organismeFormation"))
            {
                organismeFormateurItems = dico["organismeFormation"].Valeur.Split(FILTER_SEPARATOR);
                if (organismeFormateurItems.Length == 0)
                {
                    libOrganismeFormateur = "[Prestataire].[Prestataire].[Prestataire],";
                }
                else
                {
                    if (organismeFormateurItems.Length == 1 && organismeFormateurItems[0] == String.Empty)
                        libOrganismeFormateur = "[Prestataire].[Prestataire].[Prestataire],";

                    else
                    {
                        foreach (string str in organismeFormateurItems)
                        {
                            libOrganismeFormateur += "[Prestataire].[Prestataire].[Prestataire].[" + str + "],";
                        }

                    }

                }
            }

            if (filtre.getAllFiltres().ContainsKey("qualification"))
            {
                qualificationItems = dico["qualification"].Valeur.Split(FILTER_SEPARATOR);

                if (qualificationItems.Length == 0)
                {
                    libQualification = "[Employe].[Metier].[Metier],";
                }
                else
                {
                    if (qualificationItems.Length == 1 && qualificationItems[0] == String.Empty) libQualification = "[Employe].[Metier].[Metier],";

                    else
                    {
                        foreach (string str in qualificationItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libQualification += "[Employe].[Metier].&[" + str + "],";
                        }
                    }

                }

            }

            if (filtre.getAllFiltres().ContainsKey("categorieSocioPro"))
            {
                categoriesociItems = dico["categorieSocioPro"].Valeur.Split(FILTER_SEPARATOR);
                if (categoriesociItems.Length == 0)
                {
                    libCatsociopro = "[CategorieSocioPro].[CategorieSocioPro].[CategorieSocioPro],";
                }
                else
                {
                    if (categoriesociItems.Length == 1 && categoriesociItems[0] == String.Empty) libCatsociopro = "[CategorieSocioPro].[CategorieSocioPro].[CategorieSocioPro],";

                    else
                    {
                        foreach (string str in categoriesociItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libCatsociopro += "[CategorieSocioPro].[CategorieSocioPro].&[" + str + "],";
                        }
                    }

                }

            }

            if (filtre.getAllFiltres().ContainsKey("situationMatrimoniale"))
            {
                situationmatItems = dico["situationMatrimoniale"].Valeur.Split(FILTER_SEPARATOR);
                if (situationmatItems.Length == 0)
                {
                    libSituationMat = "[SituationMatrimoniale].[SituationMatrimoniale].[SituationMatrimoniale],";
                }
                else
                {
                    if (situationmatItems.Length == 1 && situationmatItems[0] == String.Empty) libSituationMat = "[SituationMatrimoniale].[SituationMatrimoniale].[SituationMatrimoniale],";

                    else
                    {
                        foreach (string str in situationmatItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libSituationMat += "[SituationMatrimoniale].[SituationMatrimoniale].&[" + str + "],";
                        }
                    }

                }

            }

            if (filtre.getAllFiltres().ContainsKey("recruteur"))
            {
                recruteurItems = dico["recruteur"].Valeur.Split(FILTER_SEPARATOR);
                if (recruteurItems.Length == 0)
                {
                    libRecruteur = "[Recruteur].[NomRecruteur].[NomRecruteur]";
                }
                else
                {
                    if (recruteurItems.Length == 1 && recruteurItems[0] == String.Empty) libRecruteur = "[Recruteur].[NomRecruteur].[NomRecruteur],";

                    else
                    {
                        foreach (string str in recruteurItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libRecruteur += "[Recruteur].[NomRecruteur].&[" + str + "],";
                        }

                    }

                }
            }


            //if (filtre.getAllFiltres().ContainsKey("motifAbsence"))
            //{
            //    motifAbsenceItems = dico["motifAbsence"].Valeur.Split(FILTER_SEPARATOR);
            //    if (motifAbsenceItems.Length == 0)
            //    {
            //        libmotifAbsence = "[Motif Absence].[MotifAbsence].[MotifAbsence],";
            //    }
            //    else
            //    {
            //        if (motifAbsenceItems.Length == 1 && motifAbsenceItems[0] == String.Empty) libmotifAbsence = "[Motif Absence].[MotifAbsence].[MotifAbsence],";

            //        else
            //        {
            //            foreach (string str in motifAbsenceItems)
            //            {
            //                libmotifAbsence += "[Motif Absence].[MotifAbsence].&[" + str + "],";
            //            }
            //        }
            //    }
            //}

            if (filtre.getAllFiltres().ContainsKey("typeAbsence"))
            {
                typeAbsenceItems = dico["typeAbsence"].Valeur.Split(FILTER_SEPARATOR);
                if (typeAbsenceItems.Length == 0)
                {
                    libtypeAbsence = "[TypeAbsence].[TypeAbsence].[TypeAbsence],";
                }
                else
                {
                    if (typeAbsenceItems.Length == 1 && typeAbsenceItems[0] == String.Empty) libtypeAbsence = "[TypeAbsence].[TypeAbsence].[TypeAbsence],";

                    else
                    {
                        foreach (string str in typeAbsenceItems)
                        {
                            libtypeAbsence += "[TypeAbsence].[TypeAbsence].&[" + str + "],";
                        }
                    }
                }
            }


            //if (filtre.getAllFiltres().ContainsKey("entiteAdmin"))
            //{

            //    entiteAdminItems = dico["entiteAdmin"].Valeur.Split(FILTER_SEPARATOR);
            //    if (entiteAdminItems.Length == 0)
            //    {
            //        libEntiteAdmin = "[EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],";
            //    }
            //    else
            //    {
            //        if (entiteAdminItems.Length == 1 && entiteAdminItems[0] == String.Empty) libEntiteAdmin = "[EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin],";

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

            if (filtre.getAllFiltres().ContainsKey("entiteAdmin"))
            {

                entiteAdminItems = dico["entiteAdmin"].Valeur.Split(FILTER_SEPARATOR);
                if (entiteAdminItems.Length == 0)
                {
                    libEntiteAdmin = "[EntiteAdmin].[EntiteAdminParent].LEVELS(1),";
                }
                else
                {
                    if (entiteAdminItems.Length == 1 && entiteAdminItems[0] == String.Empty) libEntiteAdmin = "[EntiteAdmin].[EntiteAdminParent].LEVELS(1),";

                    else
                    {
                        foreach (string str in entiteAdminItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libEntiteAdmin += "[EntiteAdmin].[EntiteAdminParent].[" + str + "],";
                        }
                    }

                }
            }

            if (filtre.getAllFiltres().ContainsKey("trancheAge"))
            {
                trancheAgeItems = dico["trancheAge"].Valeur.Split(FILTER_SEPARATOR);
                if (trancheAgeItems.Length == 0)
                {
                    libTrancheAge = "[TrancheAge].[TrancheAge].[TrancheAge],";
                }
                else
                {
                    if (trancheAgeItems.Length == 1 && trancheAgeItems[0] == String.Empty) libTrancheAge = "[TrancheAge].[TrancheAge].[TrancheAge],";

                    else
                    {
                        foreach (string str in trancheAgeItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libTrancheAge += "[TrancheAge].[TrancheAge].[TrancheAge].[" + str + "],";
                        }
                    }

                }
            }


            if (filtre.getAllFiltres().ContainsKey("trancheAnciennete"))
            {
                trancheAncienneteItems = dico["trancheAnciennete"].Valeur.Split(FILTER_SEPARATOR);
                if (trancheAncienneteItems.Length == 0)
                {
                    libTrancheAnciennete = "[TrancheAnciennete].[TrancheAnciennete].[TrancheAnciennete],";
                }
                else
                {
                    if (trancheAncienneteItems.Length == 1 && trancheAncienneteItems[0] == String.Empty) libTrancheAnciennete = "[TrancheAnciennete].[TrancheAnciennete].[TrancheAnciennete],";

                    else
                    {
                        foreach (string str in trancheAncienneteItems)
                        {
                            //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                            libTrancheAnciennete += "[TrancheAnciennete].[TrancheAnciennete].[TrancheAnciennete].[" + str + "],";
                        }

                    }

                }
            }


            if (filtre.getAllFiltres().ContainsKey("sexe"))
            {
                if (sexeItems.Equals("all"))
                {
                    libSexe = "[Sexe].[LibelleSexe].[LibelleSexe]";
                }
                else
                {
                    //[EntiteAdmin].[LibEntiteAdmin].&[Premier ministère]
                    libSexe += "[Sexe].[LibelleSexe].[" + sexeItems + "]";
                }

            }



            if (anneeItem == null || anneeItem == String.Empty) libAnnee = "{[Temps].[Calendar Year].&[" + currentYear() + "]},";
            else libAnnee = "{[Temps].[Calendar Year].&[" + anneeItem + "]},";




            switch (typeOfCondition)
            {

                case "type1": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";

                    break;

                case "type2": //where qui prend en compte les entites admin, tranche age,anciennete, sexe et l'annee

                    whereCondition = "WHERE ({"
                          + libAnnee.Substring(0, libAnnee.Length - 1)
                          + "}, {"
                          + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                          + "}, {"
                          + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                          + "}, {"
                          + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                          + "}, {"
                          + libSexe
                          + "});";

                    break;

                case "type3":

                    whereCondition = "WHERE ({"
                        + libAnnee.Substring(0, libAnnee.Length - 1)
                        + "}, {"
                        + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                        + "}, {"
                        + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                        + "}, {"
                        + libSexe
                        + "});";

                    break;

                case "type4": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSourceRecrutement.Substring(0, libSourceRecrutement.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";

                    break;

                case "type5": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                        //+ libTypeFormation.Substring(0, libTypeFormation.Length - 1)
                        //+ "}, {"
                           + libSexe
                           + "}, {"
                           + libOrganismeFormateur.Substring(0, libOrganismeFormateur.Length - 1)
                           + "}, {"
                           + libDomaine.Substring(0, libDomaine.Length - 1)
                           + "}";

                    break;

                case "type6": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                        //+ libTypeFormation.Substring(0, libTypeFormation.Length - 1)
                        //+ "}, {"
                           + libSexe
                           + "}, {"
                           + libOrganismeFormateur.Substring(0, libOrganismeFormateur.Length - 1)
                           + "},";

                    break;

                case "type7": //where qui prend en compte les entites admin, tranche age,anciennete,typecontrat,catsocio pro, sit mat

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libSexe
                           + "}, {"
                           + libCatsociopro.Substring(0, libCatsociopro.Length - 1)
                           + "}, {"
                           + libSituationMat.Substring(0, libSituationMat.Length - 1)
                           + "});";

                    break;

                case "type8": //prend en compte date entites admin, tran age,anciennete,typecontrat,catsocio pro, sit mat

                    whereCondition = "WHERE ({"
                           + libAnnee.Substring(0, libAnnee.Length - 1)
                           + "}, {"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libSexe
                           + "}, {"
                           + libCatsociopro.Substring(0, libCatsociopro.Length - 1)
                           + "}, {"
                           + libSituationMat.Substring(0, libSituationMat.Length - 1)
                           + "});";

                    break;
                case "type9": //where qui prend en compte les entites admin, tranche age,anciennete, sexe, Emploi type, sourcerecrutement,recruteur

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSourceRecrutement.Substring(0, libSourceRecrutement.Length - 1)
                           + "}, {"
                           + libRecruteur.Substring(0, libRecruteur.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";

                    break;

                case "type10": // Where du dashboard de JPC correspondant au type1
                    whereCondition = "WHERE ({"
                        + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libtypeAbsence.Substring(0, libtypeAbsence.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;

                case "type11": //prend en compte date entites admin, tran age,anciennete,typecontrat,catsocio pro, sit mat, hierarchietemps

                    whereCondition = "WHERE ({"
                        //+ libAnnee.Substring(0, libAnnee.Length - 1)
                        //+ "}, {"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                        //+ "}, {"
                        //+ libSexe
                           + "}, {"
                           + libCatsociopro.Substring(0, libCatsociopro.Length - 1)
                           + "}, {"
                           + libSituationMat.Substring(0, libSituationMat.Length - 1)
                           + "}";

                    break;

                case "type12"://where qui prend en compte les entites admin, tranche age,anciennete, sexe, Emploi type, sourcerecrutement,recruteur
                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSourceRecrutement.Substring(0, libSourceRecrutement.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;

                case "type13": //prend en compte date entites admin, tran age,anciennete,typecontrat,catsocio pro,sexe sit mat, hierarchietemps

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libSexe
                           + "}, {"
                           + libCatsociopro.Substring(0, libCatsociopro.Length - 1)
                           + "}, {"
                           + libSituationMat.Substring(0, libSituationMat.Length - 1)
                           + "}";

                    break;

                case "type14": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";

                    break;

                case "type15": //where qui prend en compte les entites admin, tranche age,anciennete, sexe

                    whereCondition = "WHERE ({"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";

                    break;

                case "type20"://where du dashboard de JPC correspondant au type 2
                    whereCondition = "WHERE ({"
                         + this.getTimeRowQuery(filtre)
                          + "}, {"
                          + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libtypeAbsence.Substring(0, libtypeAbsence.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;

                case "type21"://where du dashboard de JPC correspondant au type 2
                    whereCondition = "WHERE ({"
                           + libEntiteAdmin.Substring(0, libEntiteAdmin.Length - 1)
                           + "}, {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libtypeAbsence.Substring(0, libtypeAbsence.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;

                case "type30":// where du JPC correspondant au type 3
                    whereCondition = "WHERE ({"
                          + this.getTimeRowQuery(filtre)
                           + "} , {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;


                case "type31":// where du JPC correspondant au type 3
                    whereCondition = "WHERE ({"
                         + "[Temps].[Calendar Year].&[" + annee + "]} , {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;

                case "type40":
                    whereCondition = "WHERE ({"
                         + this.getTimeRowQuery(filtre)
                          + "} , {"
                           + libTrancheAge.Substring(0, libTrancheAge.Length - 1)
                           + "}, {"
                           + libTrancheAnciennete.Substring(0, libTrancheAnciennete.Length - 1)
                           + "}, {"
                           + libTypeContrat.Substring(0, libTypeContrat.Length - 1)
                           + "}, {"
                           + libEmploiType.Substring(0, libEmploiType.Length - 1)
                           + "}, {"
                           + libSexe
                           + "});";
                    break;



            }

            return whereCondition;
        }

        #endregion 

        private string getTimeRowQuery(FiltreDashboard filtre)
        {
            string rowQuery = String.Empty;
            string annee = String.Empty;
            string semester = String.Empty;
            string quater = String.Empty;
            string month = String.Empty;

            DateTime maintenant = DateTime.Now;
            int year = maintenant.Year;
            int semestre = (maintenant.Month - 1) / 6 + 1;
            int trimestre = (maintenant.Month - 1) / 3 + 1;
            int mois = maintenant.Month;

            string moisName = maintenant.ToString("MMMM", new CultureInfo("en-US"));

            annee = filtre.DicoFiltres["annee"].Valeur;
            if (filtre.getAllFiltres().ContainsKey("semestre"))
            {
                semester = filtre.DicoFiltres["semestre"].Valeur;
                // LastPeriods(4, [Temps].[Calendar Semester].&[2015]&[1])

                rowQuery = "LastPeriods(4, [Temps].[Calendar Semester].&[" + annee + "]&[" + semester + "])";
            }

            else if (filtre.getAllFiltres().ContainsKey("trimestre"))
            {
                quater = filtre.DicoFiltres["trimestre"].Valeur;
                rowQuery = "LastPeriods(4, [Temps].[Calendar Quarter].&[" + annee + "]&[" + quater + "])";
            }

            else if (filtre.getAllFiltres().ContainsKey("mois"))
            {
                if (filtre.DicoFiltres["mois"].Valeur == String.Empty)
                {
                    annee = year.ToString();
                    month = "&[" + semestre + "]&[" + trimestre + "]&[" + moisName + "]";
                }
                else month = filtre.DicoFiltres["mois"].Valeur;


                //LastPeriods(4, [Temps].[English Month Name].&[2006]&[1]&[2]&[April])

                rowQuery = "LastPeriods(4, [Temps].[English Month Name].&[" + annee + "]" + month + ")";
            }
            else
            {
                //annee = filtre.DicoFiltres["annee"].Valeur;
                rowQuery = "LastPeriods(4, [Temps].[Calendar Year].&[" + annee + "])";
            }

            return rowQuery;

        }

    }
}