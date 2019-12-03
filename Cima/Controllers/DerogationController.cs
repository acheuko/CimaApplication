using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cima.Models;
using Cima.Repository;
using Cima.Repository.Shared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cima.Controllers
{
    public class DerogationController : Controller
    {
        //
        // GET: /Derogation/

        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private readonly REPO_Campaign campaignRepository;
        private readonly REPO_Derogation repoDerogation;

        public DerogationController()
        {
            campaignRepository = (REPO_Campaign)unitOfWork.CampaignRepository;
            repoDerogation = (REPO_Derogation)unitOfWork.DerogationRepository;
        }

        public ActionResult Index()
        {
          
            return View("Derogation");
        }

        public ActionResult Derogation_Read([DataSourceRequest]DataSourceRequest request)
        {
            var derogationsQuery = repoDerogation.GetAll(
                orderBy: q => q.OrderByDescending(d => d.DerogationId));
            return Json(derogationsQuery.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Derogation_Create(string selectMotif, string selectCampagne, string selectFichier, string raison)
        {
            try
            {
                Derogation derogation = new Derogation
                {
                    Motif = selectMotif,
                    Campagne = selectCampagne,
                    Fichier = selectFichier,
                    Raison = raison,
                    Statut = "O"
                };
                repoDerogation.Insert(derogation);
                unitOfWork.Save();
                                   
                return Json(derogation, JsonRequestBehavior.AllowGet);
               
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                throw new Exception();
            }
                        
        }

    }
}
