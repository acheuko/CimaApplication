﻿using System;
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
            using (var sqlQuery = new SqlCommand(@"SELECT ID_Campaign,BeginDate,EndDate FROM sysman.tblCampaign WHERE Status = @Status", sqlConnection))
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
                                EndDate = sqlQueryResult.GetDateTime(2)
                            };

                            listcampaign.Add(campaign);
                        }

                    }
            }

            return listcampaign;
        }
    }
}