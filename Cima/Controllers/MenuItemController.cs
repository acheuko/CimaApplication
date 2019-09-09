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
    public class MenuItemController : Controller
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private readonly REPO_MenuItems menuRepository;

        public MenuItemController()
        {
            menuRepository = (REPO_MenuItems)unitOfWork.MenuRepository;
        }

        //
        // GET: /MenuItem/
        public ActionResult Index()
        {
            PopulateMenusDropDownList();
            return View("MenuItem");
        }

        public ActionResult MenuItem_Read([DataSourceRequest]DataSourceRequest request)
        {
            var menuItems = menuRepository.GetAll();
            return Json(menuItems.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /MenuItem/Details/5
        public ViewResult Details(int id)
        {
            Cima.Models.MenuItem menuItem = menuRepository.GetByID(id);
            return View(menuItem);
        }

        //
        // GET: /MenuItem/Create

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult MenuItem_Create([DataSourceRequest] DataSourceRequest request,
        [Bind(Include = "Name,Controller,Action,MenuParentId,Icon,ParamUrl")]
        Models.MenuItem menu)
        {
            List<Models.MenuItem> results = new List<Models.MenuItem>();
            try
            {
                if (ModelState.IsValid)
                {
                    menu.Controller = menu.Controller ?? "";
                    menu.Action = menu.Action ?? "";
                    menuRepository.Insert(menu);
                    unitOfWork.Save();

                    results = new List<Models.MenuItem>
                    {
                        menu
                    };
                    return Json(results.ToDataSourceResult(request, ModelState));
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateMenusDropDownList(menu.MenuParentId);
            return Json(results.ToDataSourceResult(request, ModelState));
        }

       

        private void PopulateMenusDropDownList(object selectedMenu = null)
        {
            var menusQuery = menuRepository.GetAll(
                orderBy: q => q.OrderBy(d => d.Name));
            ViewBag.MenusParent = menusQuery; //new SelectList(menusQuery, "MenuId", "Name", selectedMenu);
        }

        //
        // POST: /MenuItem/Update/5
        [HttpPost]
        public ActionResult MenuItem_Update([DataSourceRequest] DataSourceRequest request,
             Models.MenuItem menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    menu.Controller = menu.Controller ?? "";
                    menu.Action = menu.Action ?? "";
                    menuRepository.Update(menu);
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
        // POST: /MenuItem/Delete/5
        [HttpPost]
        public ActionResult MenuItem_Delete([DataSourceRequest] DataSourceRequest request, Models.MenuItem menu)
        {
            try
            {
                if (menu != null)
                    menuRepository.Delete(menu);
                unitOfWork.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save delete changes. Try again, and if the problem persists, see your system administrator.");
            }

            return Json("".ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}
