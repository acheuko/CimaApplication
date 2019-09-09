using Cima.Models;
using Cima.Repository;
using Cima.Repository.Shared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Cima.Controllers
{
    public class ProfilUserController : Controller
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private readonly REPO_ProfilUser profilRepository;
        private readonly REPO_MenuItems repoMenuItems;

        public ProfilUserController()
        {
            profilRepository = (REPO_ProfilUser)unitOfWork.ProfilRepository;
            repoMenuItems = (REPO_MenuItems)unitOfWork.MenuRepository;
        }

        //
        // GET: /Profil/

        public ViewResult Index()
        {
            // Récupérer la liste des menus
            GetMenuItemSelectList();

            return View("ProfilUser");
        }

        public ActionResult ProfilUser_Read([DataSourceRequest]DataSourceRequest request)
        {
            var profilUsers = profilRepository.GetAll();
            return Json(profilUsers.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProfilUser_GetAll()
        {
            var profilUsers = profilRepository.GetAll();
            return Json(profilUsers, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Profil/Details/5

        public ViewResult Details(int id)
        {
            ProfilUser profil = profilRepository.GetByID(id);
            return View(profil);
        }

        //
        // GET: /Profil/Create

        public ActionResult Create()
        {   
            return View();
        }

        [HttpPost]
        public ActionResult ProfilUser_Create([DataSourceRequest] DataSourceRequest request,
         [Bind(Include = "WebFolder,ProfilName")]
         ProfilUser profil)
        {
            List<ProfilUser> results = new List<ProfilUser>();
            try
            {
                if (ModelState.IsValid)
                {
                    profilRepository.Insert(profil);
                    unitOfWork.Save();
                    results.Add(profil);

                    return Json(results.ToDataSourceResult(request, ModelState));
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
           
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        //
        // POST: /Profil/Update/5
        [HttpPost]
        public ActionResult ProfilUser_Update([DataSourceRequest] DataSourceRequest request,
             ProfilUser profilUser)
        {
            try
            {
                if (ModelState.IsValid)
                {                
                    profilRepository.Update(profilUser);
                }

                unitOfWork.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save update changes. Try again, and if the problem persists, see your system administrator.");
            }

            return Json("".ToDataSourceResult(request, ModelState));
        }


        //
        // POST: /Profil/Delete/5

        [HttpPost]
        public ActionResult ProfilUser_Delete([DataSourceRequest] DataSourceRequest request, ProfilUser profilUser)
        {          
            if (profilUser != null)
                profilRepository.Delete(profilUser);
            unitOfWork.Save();

            return Json("".ToDataSourceResult(request, ModelState));
        }

        private void GetMenuItemSelectList()
        {
            var menusQuery = repoMenuItems.GetAll(
                orderBy: q => q.OrderBy(d => d.Name));
            ViewBag.MenuItems = menusQuery.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}
