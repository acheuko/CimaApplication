using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cima.Models;
using Cima.Repository;
using Cima.Repository.Shared;

namespace Cima.Controllers
{
    public class ProfilPrivilegeController : Controller
    {
        protected const string JSON_RESULT_FAILURE = "FAILURE";
        protected const string JSON_RESULT_SUCCESS = "SUCCESS";

        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private readonly REPO_ProfilePrivilege profilPrivilegeRepository;

        public ProfilPrivilegeController()
        {
            profilPrivilegeRepository = (REPO_ProfilePrivilege)unitOfWork.ProfilPrivilegeRepository;
        }

        //
        // GET: /ProfilPrivilege/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SaveProfilPrivilege(int profilUserId, string[] menuItems)
        {
            List<MenuItem> availableMenuItems = null;
            if (profilUserId != -1)
            {
                // suppression de tous les affections du profil sélectionné
                int deleted = profilPrivilegeRepository.DeletePrivilegeByProfilId(profilUserId);
                if (deleted != -1 && menuItems != null && menuItems.Length > 0)
                {
                    // insertion des privileges du profil selectionné
                    foreach (var m in menuItems)
                    {
                        ProfilPrivilege profilPrivilege = new ProfilPrivilege
                        {
                            ProfilUserId = profilUserId.ToString(),
                            MenuItemId = m.ToString()
                        };
                        try
                        {
                            profilPrivilegeRepository.Insert(profilPrivilege);
                            unitOfWork.Save();

                            availableMenuItems = GetMenuItemSelectList();

                        }
                        catch (DataException /* dex */)
                        {
                            //Status = JSON_RESULT_FAILURE;
                            //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }

                    }
                }
                
            }
            return Json(availableMenuItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMenuItemByProfilId(int profilUserId)
        {
            List<MenuItem> selectedMenuItems = new List<MenuItem>();
            if (profilUserId != -1)
            {
                selectedMenuItems = profilPrivilegeRepository.GetMenuItemsByProfilId(profilUserId);
            }

            List<MenuItem> availableMenuItems = GetMenuItemSelectList();

            Dictionary<string, List<MenuItem>> dicoMenuItems = new Dictionary<string, List<MenuItem>>
            {
                { "availableMenuItems", availableMenuItems },
                { "selectedMenuItems", selectedMenuItems }
            };

            return Json(dicoMenuItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableMenuItems()
        {
            var availableMenuItems = GetMenuItemSelectList();

            return Json(availableMenuItems, JsonRequestBehavior.AllowGet);
        }

        private List<MenuItem> GetMenuItemSelectList()
        {
            var repoMenuItems = (REPO_MenuItems)unitOfWork.MenuRepository;

            var menusQuery = repoMenuItems.GetAll(
                orderBy: q => q.OrderBy(d => d.Name));

            return menusQuery.ToList(); 
        }
    }
}
