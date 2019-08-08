using System;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Cima.Repository.TestData;
using Cima.Models.TestModel;
using Cima.Models.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cima.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        private FiltreDashboard filtre = new FiltreDashboard();

        public ActionResult Login()
        {
            // ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }

       

        public ActionResult Taux([DataSourceRequest]DataSourceRequest request)
        {
            //this.filtre.DicoFiltres = this.buildFiltreDashboard();

            ObservableCollection<Taux> tauxResult = new REPO_Taux().GetTauxRecrutementData(filtre);
            //DataSourceResult result = tauxResult.ToDataSourceResult(request, taux => new
            //{

            //});

            return Json(tauxResult, JsonRequestBehavior.AllowGet);
        }

      

        public ActionResult Dashboard3()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public JsonResult CatSocioPro_Read()
        {

            

           // this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<BusinessModel>> criteriaItems = new REPO_CriteriaItems().GetFilterData(filtre);
            ObservableCollection<BusinessModel> catSocioPro = new ObservableCollection<BusinessModel>();

            catSocioPro = criteriaItems["categorieSocioPro"];

            return Json(catSocioPro, JsonRequestBehavior.AllowGet);

        }

        public JsonResult TypeContrat_Read()
        {

           
            Dictionary<string, ObservableCollection<BusinessModel>> criteriaItems = new REPO_CriteriaItems().GetFilterData(filtre);
            ObservableCollection<BusinessModel> typeContrats = new ObservableCollection<BusinessModel>();

            typeContrats = criteriaItems["typeContrat"];

            return Json(typeContrats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TrancheAnc_Read()
        {
            //this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<BusinessModel>> criteriaItems = new REPO_CriteriaItems().GetFilterData(new FiltreDashboard());
            ObservableCollection<BusinessModel> trancheAncs = new ObservableCollection<BusinessModel>();

            trancheAncs = criteriaItems["trancheAnciennete"];

            return Json(trancheAncs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TableDetails_Read([DataSourceRequest]DataSourceRequest request,
            string catSocioPro,
            string typeContrat,
            string trancheAnc)
        {

           
            this.filtre.DicoFiltres = this.buildFiltreDashboard(catSocioPro,typeContrat,trancheAnc);

            List<GridDetails> gridDetails = new List<GridDetails>();
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15, 00, 00, 32), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Abaya", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" });
            
            return Json(gridDetails.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TableTotal_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<GridTotal> gridTotals = new List<GridTotal>();

            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Incoming", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "intra-BSC", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Abaya", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Udeni", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "inter-BSC", Site = "Abuja", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });
            gridTotals.Add(new GridTotal { CallCategory = "Outgoing", Site = "Chaichai", MonthlyTotal = 864.24, AvgCallDurationIn = 125.23, AvgCallDurationOut = 254.236, AvgNumberCallIn = 0214.32, AvgNumberCallOut = 45.236 });

            return Json(gridTotals.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        private Dictionary<string, FiltreElement> buildFiltreDashboard(string catSocioPro,string typeContrat,string trancheAnc)
        {
            Dictionary<string, FiltreElement> filterItems = new Dictionary<string, FiltreElement>();


            filterItems.Add("catSocioPro", new FiltreElement { Nom = "catSocioPro", Valeur = catSocioPro });
            filterItems.Add("typeContrat", new FiltreElement { Nom = "typeContrat", Valeur = typeContrat });
            filterItems.Add("trancheAnc", new FiltreElement { Nom = "trancheAnc", Valeur = trancheAnc });
           

            return filterItems;
        }

    }
}
