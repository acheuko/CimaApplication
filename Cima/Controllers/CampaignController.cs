using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cima.Helpers;
using Cima.Models;
using Cima.Repository;
using Cima.Repository.Shared;
using Cima.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cima.Controllers
{
    public class CampaignController : Controller
    {

        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private readonly REPO_CampaignFile campaignFileRepository;
        private readonly REPO_Campaign campaignRepository;

        public CampaignController()
        {
            campaignRepository = (REPO_Campaign)unitOfWork.CampaignRepository;
            campaignFileRepository = (REPO_CampaignFile)unitOfWork.CampaignFileRepository;
        }

        //
        // GET: /Campaign/

        public ActionResult Index()
        {
            ViewBag.CampaignFiles = GetCampaignFilesAvailableList();

            return View("Campaign");
        }

        public ActionResult ReadCampaign([DataSourceRequest]DataSourceRequest request)
        {
            var campaignItems = campaignRepository.GetCampaigns();
            return Json(campaignItems.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = Profil.ADMIN)]
        public ActionResult CreateCampaign(DateTime datedeb, DateTime datefin, string[] selectedCampaignFiles, string[] selectedCampaingControls, 
                                string optexhaustivite, string optcoherence, string optconformite,
                                string annee, string periode, string libperiode,string nom, string code)
        {

            string libPeriodeLong = PeriodeHelper.GetLibelleLong(libperiode, periode);

            Campaign c = new Campaign
            {
                Year = annee,
                Periode = periode,
                LibPeriodeCourt = libperiode +" "+ annee,
                LibPeriodeLong = libPeriodeLong + " "+ annee,
                BeginDate = datedeb,
                EndDate = datefin,
                Status = "O",
                CreationDate = DateTime.Now,
                Nom = nom,
                Code = code,
                LibelleCampagne = nom + "_" + code + "_" + libPeriodeLong + "_" + annee
            };

            c = campaignRepository.InsertAndReturn(c);

            unitOfWork.Save();

            campaignRepository.SaveCampaignFile(c.CampaignId, selectedCampaignFiles);

            campaignRepository.SaveCampaignControl(c.CampaignId, selectedCampaingControls, optexhaustivite,  optcoherence,  optconformite);

            ObservableCollection<CampaignFile> campaignFiles = campaignFileRepository.GetAll();

            Dictionary<String, Object> response = new Dictionary<string, object>
            {
                ["status"] = "SUCCESS",
                ["data"] = campaignFiles
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFilesByCampaignId(int IdCampaign)
        {
            REPO_CampaignCampaignFile repoCampaignFiles = new REPO_CampaignCampaignFile();

            ObservableCollection<CampaignFile> result = repoCampaignFiles.GetFilesByCampaignId(IdCampaign);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private List<CampaignFile> GetCampaignFilesAvailableList()
        {
            var campaignFilesQuery = campaignFileRepository.GetAll(
                orderBy: q => q.OrderBy(d => d.CampaignFileName));

            return campaignFilesQuery.ToList();
        }

      

    }
}
