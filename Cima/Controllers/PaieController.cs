using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Cima.Repository.TestData;
using Cima.Models.TestModel;
using Cima.Models.Shared;

namespace Cima.Controllers
{
   // [Authorize]
    public class PaieController : Controller
    {

        IREPO_Paie repo = REPO_Paie.getRepository();
        private FiltreDashboard filtre = new FiltreDashboard();
        
        public PaieController()
        {

        }

        //
        // GET: /Paie/

        //[Authorize]
        public ActionResult Paie()
        {
            
            return View();
        }

        public JsonResult EntiteAdmin_Read()
        {
            // this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<PaieCriteria>> criteriaItems = repo.GetFilterData(new FiltreDashboard());
            ObservableCollection<PaieCriteria> entiteAdmins = new ObservableCollection<PaieCriteria>();

            entiteAdmins = criteriaItems["entiteAdmin"];
            entiteAdmins.Insert(0, new PaieCriteria { Xvalue = "-- All" });

            return Json(entiteAdmins, JsonRequestBehavior.AllowGet);

        }

        public JsonResult TrancheAge_Read()
        {
            // this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<PaieCriteria>> criteriaItems = repo.GetFilterData(new FiltreDashboard());
            ObservableCollection<PaieCriteria> tranchesAge = new ObservableCollection<PaieCriteria>();

            tranchesAge = criteriaItems["trancheAge"];
            tranchesAge.Insert(0, new PaieCriteria { Xvalue = "-- All" });

            return Json(tranchesAge, JsonRequestBehavior.AllowGet);

        }

        public JsonResult TrancheAnc_Read()
        {
            //this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<PaieCriteria>> criteriaItems =   repo.GetFilterData(new FiltreDashboard());
            ObservableCollection<PaieCriteria> trancheAncs = new ObservableCollection<PaieCriteria>();

            trancheAncs = criteriaItems["trancheAnciennete"];
            trancheAncs.Insert(0, new PaieCriteria { Xvalue = "-- All" });

            return Json(trancheAncs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="temps"></param>
        /// <param name="catSocioPro"></param>
        /// <param name="typeContrat"></param>
        /// <param name="trancheAnc"></param>
        /// <param name="sexe"></param>
        /// <returns></returns>
        public JsonResult GetEffectifanneeItems([DataSourceRequest]DataSourceRequest request,
           string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps=null)
        {
           // if (temps == null) temps = DateTime.Now;

            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe,temps);
            ObservableCollection<Effectif> effectifParAnnee = repo.GetNumberofEmployeesbyYear(filtre);


            return Json(effectifParAnnee, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTop5EffectifDptItems([DataSourceRequest]DataSourceRequest request,
           string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps = null)
        {
            //if (temps == null) temps = DateTime.Now;
            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe, temps);
            ObservableCollection<Effectif> effectifDpt = repo.GetHeadcount(filtre);

            return Json(effectifDpt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEvolMasseSalarialeItems([DataSourceRequest]DataSourceRequest request, string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps = null)
        {


            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe, temps);
            ObservableCollection<Montant> evolMasseSalariale = repo.GetSalaryGraph(filtre);

            return Json(evolMasseSalariale, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEffectifCSPItems([DataSourceRequest]DataSourceRequest request, string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps = null)
        {
            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe, temps);
            ObservableCollection<Effectif> effectifCSP = repo.GetNumberofEmployeesbySalary(filtre);

            return Json(effectifCSP, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalaireEntAdminItems([DataSourceRequest]DataSourceRequest request, string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps = null)
        {
            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe, temps);
            ObservableCollection<Montant> salaireEntAdmin = repo.GetPayrollBreakdown(filtre);

            return Json(salaireEntAdmin, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMontantDeductionItems([DataSourceRequest]DataSourceRequest request, string entiteadmin,
           string trancheAge,
           string trancheAnc,
           string sexe,
           DateTime? temps = null)
        {
            this.filtre.DicoFiltres = this.buildFiltreDashboard(entiteadmin, trancheAge, trancheAnc, sexe, temps);
            ObservableCollection<Montant> montantDeduction = repo.GetTaxDeductionGraph(filtre);

            return Json(montantDeduction, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tps"></param>
        /// <param name="catSocioPro"></param>
        /// <param name="typeContrat"></param>
        /// <param name="trancheAnc"></param>
        /// <param name="sexeValue"></param>
        /// <returns></returns>
        private Dictionary<string, FiltreElement> buildFiltreDashboard(string entiteAdmin, string trancheAge, string trancheAnc, string sexeValue,DateTime? tps=null)
        {
            Dictionary<string, FiltreElement> filterItems = new Dictionary<string, FiltreElement>();

            if (!tps.HasValue) tps = DateTime.Now.AddMonths(-1);
            
            int monthNumberOfYear = tps.Value.Month;
            string monthName=String.Empty;

            if (monthNumberOfYear == 1) monthName = "&[1]&[1]&[January]";
            if (monthNumberOfYear == 2) monthName = "&[1]&[1]&[February]";
            if (monthNumberOfYear == 3) monthName = "&[1]&[1]&[March]";
            if (monthNumberOfYear == 4) monthName = "&[1]&[2]&[April]";
            if (monthNumberOfYear == 5) monthName = "&[1]&[2]&[May]";
            if (monthNumberOfYear == 6) monthName = "&[1]&[2]&[June]";
            if (monthNumberOfYear == 7) monthName = "&[2]&[3]&[July]";
            if (monthNumberOfYear == 8) monthName = "&[2]&[3]&[August]";
            if (monthNumberOfYear == 9) monthName = "&[2]&[3]&[September]";
            if (monthNumberOfYear == 10) monthName = "&[2]&[4]&[October]";
            if (monthNumberOfYear == 11) monthName = "&[2]&[4]&[November]";
            if (monthNumberOfYear == 12) monthName = "&[2]&[4]&[December]";

            if (entiteAdmin == null || entiteAdmin.Equals("-- All")) entiteAdmin = String.Empty;
            if (trancheAge == null || trancheAge.Equals("-- All")) trancheAge = String.Empty;
            if (trancheAnc == null || trancheAnc.Equals("-- All")) trancheAnc = String.Empty;

            filterItems.Add("annee", new FiltreElement { Nom = "annee", Valeur = tps.Value.Year.ToString() });
            filterItems.Add("mois", new FiltreElement { Nom = "mois", Valeur = monthName });
            filterItems.Add("entiteAdmin", new FiltreElement { Nom = "entiteAdmin", Valeur = entiteAdmin });
            filterItems.Add("trancheAge", new FiltreElement { Nom = "trancheAge", Valeur = trancheAge });
            filterItems.Add("trancheAnciennete", new FiltreElement { Nom = "trancheAnciennete", Valeur = trancheAnc });
            filterItems.Add("sexe", new FiltreElement { Nom = "sexe", Valeur = sexeValue });


            return filterItems;
        }

    }
}
