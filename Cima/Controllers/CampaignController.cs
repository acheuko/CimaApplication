using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cima.Helpers;
using Cima.Models;
using Cima.Repository;
using Cima.Repository.Shared;
using Cima.ViewModel;

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
            GetCampaignFilesAvailableList();

            return View("Campaign");
        }


        [Authorize(Roles = Profil.ADMIN)]
        public ActionResult CreateCampaign(DateTime datedeb, DateTime datefin, string[] selectedCampaignFiles, string[] selectedCampaingControls, 
                                string optexhaustivite, string optcoherence, string optconformite,
                                string annee, string periode, string libperiode)
        {

            Campaign c = new Campaign
            {
                Year = annee,
                Periode = periode,
                LibPeriodeCourt = libperiode +" "+ annee,
                LibPeriodeLong = PeriodeHelper.GetLibelleLong(libperiode, periode) +" "+ annee,
                BeginDate = datedeb,
                EndDate = datefin,
                Status = "O",
                CreationDate = DateTime.Now
            };

            c = campaignRepository.InsertAndReturn(c);

            unitOfWork.Save();

            campaignRepository.SaveCampaignFile(c.CampaignId, selectedCampaignFiles);

            campaignRepository.SaveCampaignControl(c.CampaignId, selectedCampaingControls, optexhaustivite,  optcoherence,  optconformite);

            return Json(c, JsonRequestBehavior.AllowGet);
        }


        private void GetCampaignFilesAvailableList()
        {
            var campaignFilesQuery = campaignFileRepository.GetAll(
                orderBy: q => q.OrderBy(d => d.CampaignFileName));
            ViewBag.CampaignFiles = campaignFilesQuery.ToList();
        }

      

    }
}
