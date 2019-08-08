using System;
using Cima.Models.Shared;
using Cima.Models.TestModel;
using Cima.Repository.Shared;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Generic;
using Microsoft.AnalysisServices.AdomdClient;

namespace Cima.Repository.TestData
{
    public class REPO_CriteriaItems : BaseRepository<BusinessModel>
    {
        public string buildFiltreDataQuery(FiltreDashboard filtre, string bloc="")
        {
            string query = String.Empty;
            string RmUnknow = ".CurrentMember.Name<>'UNKNOW')";
            string RmUnknEntAdmin = ".CurrentMember.Name<>'UNKNOWN')";
            switch (bloc)
            {
                case "annee": //Annee
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                                "lastPeriods(5,[Temps].[Calendar Year].&[" + this.currentYear() + "] ) on 1 " +
                                 "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "semestre": //Semestre
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                                "{[Temps].[Calendar Year].&[" + this.currentYear() + "]*[Temps].[Hierarchie Temps].[Calendar Semester]} on 1 " +
                                "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "trimestre": //Trimestre
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                               "{[Temps].[Calendar Year].&[" + this.currentYear() + "]*[Temps].[Hierarchie Temps].[Calendar Quarter]} on 1 " +
                               "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "mois": //Mois
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                               "{[Temps].[Calendar Year].&[" + this.currentYear() + "]*[Temps].[Hierarchie Temps].[English Month Name]} on 1 " +
                               "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "entiteAdmin": //EntiteAdmin
                    {
                        query = "select [Measures].[SalaireNet] on 0, " +
                               "[EntiteAdmin].[LibEntiteAdmin].[LibEntiteAdmin] on 1 " +
                               "from [SBI_Cube_Paie]";
                        break;
                    }
                case "trancheAge": //TrancheAge
                    {
                        query = "select [Measures].[SalaireNet] on 0, " +
                               "[TrancheAge].[TrancheAge].[TrancheAge] on 1 " +
                               "from [SBI_Cube_Paie]";
                        break;
                    }
                case "trancheAnciennete": //TrancheAnciennete
                    {
                        query = "select [Measures].[SalaireNet] on 0, " +
                               "[TrancheAnciennete].[TrancheAnciennete].[TrancheAnciennete] on 1 " +
                               "from [SBI_Cube_Paie]";
                        break;
                    }
                case "typeContrat": //TypeContrat
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                                 "filter([TypeContrat].[TypeContrat].[TypeContrat],[TypeContrat].[TypeContrat]" + RmUnknow + " on 1 " +
                               "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "emploiType": //emploiType
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                               "filter([EmploiType].[EmploiType].[EmploiType],[EmploiType].[EmploiType]" + RmUnknow + " on 1 " +
                               "from [SBI_Cube_Recrutement]";
                        break;
                    }
                case "sourceRecrutement": //sourceRecrutement
                    {
                        query = "select [Measures].[NombreEmbauche] on 0, " +
                               "filter([SourceRecrutement].[SourceRecrutement].[SourceRecrutement],[SourceRecrutement].[SourceRecrutement]" + RmUnknow + " on 1 " +
                               "from [SBI_Cube_Recrutement]";
                        break;
                    }
                //case "typeFormation": //TypeFormation
                //    {
                //        query = "select [Measures].[NombreDemandeFormation] on 0, " +
                //               //"filter([Type Formation].[LibelleTypeFormation].[LibelleTypeFormation],[Type Formation].[LibelleTypeFormation]" + RmUnknow + " on 1 " +
                //                 "filter([EmploiType].[EmploiType].[EmploiType],[EmploiType].[EmploiType]" + RmUnknow + " on 1 " +
                //             "from [SBI_Cube_Formation]";
                //        break;
                //    }
                case "domaineFormation": //DomaineFormation
                    {
                        query = "select [Measures].[NombreDemandeFormation] on 0, " +
                               "[CatalogueFormation].[DomaineFormation].[DomaineFormation] on 1 " +
                               "from [SBI_Cube_Formation]";
                        break;
                    }
                case "organismeFormation": //OrganismeFormation
                    {
                        query = "select [Measures].[NombreDemandeFormation] on 0, " +
                               "[Prestataire].[Prestataire].[Prestataire] on 1 " +
                               "from [SBI_Cube_Formation]";
                        break;
                    }

                //case "motifAbsence": //OrganismeFormation
                //    {
                //        query = "select [Measures].[NombreConge] on 0, " +
                //               "filter([Motif Absence].[MotifAbsence].[MotifAbsence],[Motif Absence].[MotifAbsence]" + RmUnknow + "  on 1 " +
                //               "from [SBI_Cube_TempsConge]";
                //        break;
                //    }

                case "typeAbsence": //OrganismeFormation
                    {
                        query = "select [Measures].[NbreCongeValide] on 0, " +
                               "filter([TypeAbsence].[TypeAbsence].[TypeAbsence],[TypeAbsence].[TypeAbsence]" + RmUnknow + " on 1 " +
                               "from [SBI_Cube_TempsConge]";
                        break;
                    }

                case "situationMatrimoniale": //SituationMatrimoniale
                    {
                        query = "select [Measures].[NbreEmploye] on 0, " +
                               "filter([SituationMatrimoniale].[SituationMatrimoniale].[SituationMatrimoniale],[SituationMatrimoniale].[SituationMatrimoniale]" + RmUnknEntAdmin + " on 1 " +
                               "from [SBI_Cube_GPEC]";
                        break;
                    }

                case "categorieSocioPro": //CategorieSocioPro
                    {
                        query = "select [Measures].[NbreEmploye] on 0, " +
                               "filter([CategorieSocioPro].[CategorieSocioPro].[CategorieSocioPro],[CategorieSocioPro].[CategorieSocioPro]" + RmUnknEntAdmin + " on 1 " +
                               "from [SBI_Cube_GPEC]";
                        break;
                    }

                //case "qualification": //Qualification
                //    {
                //        query = "select [Measures].[NbreEmploye] on 0, " +
                //               "filter([Employe].[Metier].[Metier],[Employe].[Metier]" + RmUnknEntAdmin + "  on 1 " +
                //               "from [SBI_Cube_GPEC]";
                //        break;
                //    }

                case "recruteur": //Recruteur 
                    {
                        query = @"SELECT {[Measures].[NombreEmbauche] } ON 0, 
                                 { ([Recruteur].[NomRecruteur].[NomRecruteur] ) } ON 1 
                                 FROM [SBI_Cube_Recrutement]";
                        break;
                    }

                case "catEntiteAdmin": //CategorieEntiteAdmin
                    {
                        query = "select {[Measures].[SalaireNet]} on 0, " +
                               "[EntiteAdmin].[EntiteAdminParent].LEVELS(1) on 1 " +
                               "from [SBI_Cube_Paie]"; break;
                    }


            }

            return query;
        }

        protected override IDbCommand GetCommand(FiltreDashboard filter,string bloc="")
        {
            AdomdCommand command = new AdomdCommand(buildFiltreDataQuery(filter,bloc));

            command.Connection = connect(CONNECTION_STRING_RASCOM);
            return command;
        }

        protected override BusinessModel MapItem(AdomdDataReader reader)
        {

            return new BusinessModel
            {
                Xvalue = reader.GetString(0)
            };
        }

        protected override ObservableCollection<BusinessModel> GetEmptyResult()
        {
            return new ObservableCollection<BusinessModel>();
        }

        public Dictionary<string, ObservableCollection<BusinessModel>> GetFilterData(FiltreDashboard filtre)
        {
            Dictionary<string, ObservableCollection<BusinessModel>> dicoFiltre = new Dictionary<string, ObservableCollection<BusinessModel>>();

            //dicoFiltre.Add("annee", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("semestre", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("trimestre", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("mois", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("trancheAge", this.Select(CONNECTION_STRING_RASCOM, filtre));
            dicoFiltre.Add("trancheAnciennete", this.Select(CONNECTION_STRING_RASCOM, filtre, "trancheAnciennete"));

            dicoFiltre.Add("typeContrat", this.Select(CONNECTION_STRING_RASCOM, filtre, "typeContrat"));
            //dicoFiltre.Add("emploiType", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("sourceRecrutement", this.Select(CONNECTION_STRING_RASCOM, filtre));

            
            //dicoFiltre.Add("domaineFormation", this.Select(CONNECTION_STRING_RASCOM, filtre));
            //dicoFiltre.Add("organismeFormation", this.Select(CONNECTION_STRING_RASCOM, filtre));

            // dicoFiltre.Add("motifAbsence", this.Select(CONNECTION_STRING_RASCOM, filtre, "motifAbsence"));
            //dicoFiltre.Add("typeAbsence", this.Select(CONNECTION_STRING_RASCOM, filtre));

            //dicoFiltre.Add("situationMatrimoniale", this.Select(CONNECTION_STRING_RASCOM, filtre));
            dicoFiltre.Add("categorieSocioPro", this.Select(CONNECTION_STRING_RASCOM, filtre, "categorieSocioPro"));
            // dicoFiltre.Add("qualification", this.Select(CONNECTION_STRING_RASCOM, filtre, "qualification"));
            //dicoFiltre.Add("recruteur", this.Select(CONNECTION_STRING_RASCOM, filtre));


            return dicoFiltre;
        }

    }
}