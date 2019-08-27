using Cima.Models.Shared;
using Cima.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cima.Controllers
{
    public class FiltreCriteriaController : Controller
    {
        //
        // GET: /FiltreCriteria/

        private readonly REPO_FiltreCriteria repoFiltreCriteria = new REPO_FiltreCriteria();
        private readonly Dictionary<string, ObservableCollection<FiltreCriteria>> CriteriaItems; 


        public FiltreCriteriaController()
        {
            CriteriaItems = repoFiltreCriteria.GetFiltreCriteriaData(); 
        }

       
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FiltreCriteria_Read(string criteriaName)
        {
           
            ObservableCollection<FiltreCriteria> filtreData = CriteriaItems[criteriaName];

            return Json(filtreData, JsonRequestBehavior.AllowGet);

        }

        // recupérer la liste de Semestres ou Trimestres
        public JsonResult FiltreCriteriaPeriod_Read(string selectPeriod)
        {
            ObservableCollection<FiltreCriteria> filtreData = null;
            if (selectPeriod != null)
            {
                if ("bt_semestre".Equals(selectPeriod))
                {
                    filtreData = new ObservableCollection<FiltreCriteria>
                    {
                        new FiltreCriteria() { TextField = "Semestre 1", ValueField = 1 },
                        new FiltreCriteria() { TextField = "Semestre 2", ValueField = 2 }
                    };
                }else if ("bt_trimestre".Equals(selectPeriod))
                {
                    filtreData = new ObservableCollection<FiltreCriteria>
                    {
                        new FiltreCriteria() { TextField = "Trimestre 1", ValueField = 1 },
                        new FiltreCriteria() { TextField = "Trimestre 2", ValueField = 2 },
                        new FiltreCriteria() { TextField = "Trimestre 3", ValueField = 3 },
                        new FiltreCriteria() { TextField = "Trimestre 4", ValueField = 4 }
                    };
                }
                
            }
           
            return Json(filtreData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCriteriaByParent(int? idParent, string childFilter, string criteriaName)
        {
            var childrenList =  repoFiltreCriteria.GetFiltreCriteriaByName(criteriaName).AsQueryable();

            if (idParent != null)
            {
                childrenList = childrenList.Where(p => p.ParentField == idParent);
            }

            if (!string.IsNullOrEmpty(childFilter))
            {
                childrenList = childrenList.Where(p => p.TextField.Contains(childFilter));
            }

            return Json(childrenList.Select(p => new { p.ValueField, p.TextField }), JsonRequestBehavior.AllowGet);
        }

    }
}
