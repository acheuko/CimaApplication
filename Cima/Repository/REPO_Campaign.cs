using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.ModelBinding;
using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;

namespace Cima.Repository
{
    public class REPO_Campaign : SqlBaseRepository<Campaign>
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        private readonly REPO_CampaignCampaignFile campaignFileRepository;
        private readonly REPO_CampaignCampaignControl campaignControlRepository;

        // Constructeur par défaut
        public REPO_Campaign()
        {
            campaignFileRepository = (REPO_CampaignCampaignFile)unitOfWork.CampaignCampaignFileRepository;
            campaignControlRepository = (REPO_CampaignCampaignControl)unitOfWork.CampaignCampaignControlRepository;
        }

        public REPO_Campaign(SysmanDbContext context) : base(context)
        {
            campaignFileRepository = (REPO_CampaignCampaignFile)unitOfWork.CampaignCampaignFileRepository;
            campaignControlRepository = (REPO_CampaignCampaignControl)unitOfWork.CampaignCampaignControlRepository;
        }

        public ObservableCollection<Campaign> GetCampaigns()
        {
            ObservableCollection<Campaign> listcampaign = new ObservableCollection<Campaign>();
            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT ID_Campaign,BeginDate,EndDate, LibPeriodeLong, Year, Periode FROM sysman.tblCampaign ORDER BY CreationDate DESC", sqlConnection))
            {
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            Campaign campaign = new Campaign()
                            {
                                CampaignId = sqlQueryResult.GetInt32(0),
                                BeginDate = sqlQueryResult.GetDateTime(1),
                                EndDate = sqlQueryResult.GetDateTime(2),
                                LibPeriodeLong = sqlQueryResult.GetString(3),
                                Year = sqlQueryResult.GetString(4),
                                Periode = sqlQueryResult.GetString(5)
                            };

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }


        /**
         * 
         */
        public void SaveCampaignControl(int idCampaign, string[] CampaignControls, string optexhaustivite, string optcoherence, string optconformite)
        {

            foreach (var control in CampaignControls)
            {
                CampaignCampaignControl campaignControl = new CampaignCampaignControl
                {
                    CampaignId = idCampaign.ToString(),
                    ControlId = control,
                    Blocking = GetblockingOption(control, optexhaustivite, optcoherence, optconformite)
                };
                try
                {
                    campaignControlRepository.Insert(campaignControl);
                    unitOfWork.Save();
                }
                catch (DataException /* dex */)
                {
                    //Status = JSON_RESULT_FAILURE;
                    //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                    Console.WriteLine("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }
        }

        private string GetblockingOption(string control, string optexhaustivite, string optcoherence, string optconformite)
        {
            string opt = "N";
            switch (control)
            {
                case "E": // exhaustivite
                    opt = optexhaustivite;
                    break;
                case "CH": // coherence
                    opt = optcoherence; 
                    break;
                case "CF": // conformite
                    opt = optconformite; 
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return opt;
        }

        public void SaveCampaignFile(int idCampaign, string[] CampaignFiles)
        {
            foreach (var c in CampaignFiles)
            {
                CampaignCampaignFile campaignFile = new CampaignCampaignFile
                {
                    CampaignId = idCampaign.ToString(),
                    FileId = c.ToString()
                };
                try
                {
                    campaignFileRepository.Insert(campaignFile);
                    unitOfWork.Save();
                }
                catch (DataException /* dex */)
                {
                    //Status = JSON_RESULT_FAILURE;
                    //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                    Console.WriteLine("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }
        }

        public ObservableCollection<Campaign> GetCampaignByStatus(string Status)
        {
            ObservableCollection<Campaign> listcampaign = new ObservableCollection<Campaign>();
            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT ID_Campaign,BeginDate,EndDate, LibPeriodeLong FROM sysman.tblCampaign WHERE Status = @Status ORDER BY CreationDate DESC", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@Status", Status);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {

                            Campaign campaign = new Campaign()
                            {
                                CampaignId = sqlQueryResult.GetInt32(0),
                                BeginDate = sqlQueryResult.GetDateTime(1),
                                EndDate = sqlQueryResult.GetDateTime(2),
                                LibPeriodeLong = sqlQueryResult.GetString(3)
                            };

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }

        public ObservableCollection<Campaign> GetAll()
        {
            ObservableCollection<Campaign> listcampaign = new ObservableCollection<Campaign>();
            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT ID_Campaign,BeginDate,EndDate, LibPeriodeLong,LibelleCampagne FROM sysman.tblCampaign WHERE EndDate > GETDATE() - 1 ORDER BY CreationDate DESC", sqlConnection))
            {
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            Campaign campaign = new Campaign()
                            {
                                CampaignId = sqlQueryResult.GetInt32(0),
                                BeginDate = sqlQueryResult.GetDateTime(1),
                                EndDate = sqlQueryResult.GetDateTime(2),
                                LibPeriodeLong = sqlQueryResult.GetString(3),
                                LibelleCampagne = sqlQueryResult.GetString(4)
                            };

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }

        public ObservableCollection<String> GetCampaignFileById(string Id)
        {
            ObservableCollection<String> listcampaign = new ObservableCollection<String>();

            if(Id == "0_0")
            {
                return listcampaign;
            }

            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT Libelle FROM sysman.tblCampaignFileMask cf JOIN sysman.tblFileMask f ON cf.ID_FileMask = f.ID_FileMask WHERE ID_Campaign = @ParamId", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@ParamId", Id);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            var campaign = sqlQueryResult.GetString(0);

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }

        public ObservableCollection<Campaign> GetCampaignById(int Id)
        {
            ObservableCollection<Campaign> listcampaign = new ObservableCollection<Campaign>();
            using (var sqlConnection = this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT BeginDate,EndDate,year,Periode,LibPeriodeCourt,Nom,Code FROM sysman.tblCampaign WHERE ID_Campaign = @Id", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@Id", Id);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {

                            Campaign campaign = new Campaign()
                            {
                                BeginDate = sqlQueryResult.GetDateTime(0),
                                EndDate = sqlQueryResult.GetDateTime(1),
                                Year = sqlQueryResult.GetString(2),
                                Periode = sqlQueryResult.GetString(3),
                                LibPeriodeCourt = sqlQueryResult.GetString(4),
                                Nom = sqlQueryResult.GetString(5),
                                Code = sqlQueryResult.GetString(6)
                            };

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }
    }
}