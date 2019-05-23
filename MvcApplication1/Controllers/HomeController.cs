using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using MvcApplication1.Repository.TestData;
using MvcApplication1.Models.TestModel;
using MvcApplication1.Models.Shared;
using System.Collections.Generic;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {

        private FiltreDashboard filtre;

        public ActionResult Login()
        {
           // ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }

        public ActionResult Index([DataSourceRequest]DataSourceRequest request)
        {
            return View();
        }

        public ActionResult Taux([DataSourceRequest]DataSourceRequest request)
        {
            this.filtre.DicoFiltres = this.buildFiltreDashboard();

            ObservableCollection<Taux> tauxResult = new REPO_Taux().GetTauxRecrutementData(filtre);
            //DataSourceResult result = tauxResult.ToDataSourceResult(request, taux => new
            //{

            //});

            return Json(tauxResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dashboard2()
        {
            ViewBag.Message = "Your app description page.";


            return View();
        }

        public ActionResult Dashboard3()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public JsonResult Country_Read()
        {

            filtre = new FiltreDashboard();

            this.filtre.DicoFiltres = this.buildFiltreDashboard();

            Dictionary<string, ObservableCollection<BusinessModel>> criteriaItems = new REPO_CriteriaItems().GetFilterData(filtre);
            ObservableCollection<BusinessModel> catSocioPro = new ObservableCollection<BusinessModel>();

            catSocioPro = criteriaItems["categorieSocioPro"];

            return Json(catSocioPro, JsonRequestBehavior.AllowGet);

            //List<string> countries = new List<string>();
            //countries.Add("Nigeria - Mobile - Other");
            //countries.Add("DRC - Mobile - Other");
            //countries.Add("Saudu Arabia");
            //countries.Add("Cameroon");
            //countries.Add("Niger");
            //countries.Add("Ghana");

            //return Json(countries, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Network_Read()
        {
            List<string> networks = new List<string>();
            networks.Add("OrangeGW");

            return Json(networks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CallType_Read()
        {
            List<string> callTypes = new List<string>();
            callTypes.Add("Outgoing");
            callTypes.Add("Incoming");
            callTypes.Add("Intra-BSC");
            callTypes.Add("Inter-BSC");

            return Json(callTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TableDetails_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<GridDetails> gridDetails = new List<GridDetails>();
            gridDetails.Add(new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15,00,00,32), PhoneNumber = " 234-80-91054665 ", Country="Nigeria - Mobile - Other",Origin="2348183833424",Duration="00:10:14" });
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
            return Json(gridDetails.ToDataSourceResult(request,ModelState),JsonRequestBehavior.AllowGet);
        }

        public ActionResult TableTotal_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<GridTotal> gridTotals = new List<GridTotal>();

            gridTotals.Add(new GridTotal { CallCategory = "Outgoing",Site="Chaichai",MonthlyTotal=864.24,AvgCallDurationIn=125.23,AvgCallDurationOut=254.236,AvgNumberCallIn=0214.32,AvgNumberCallOut=45.236 });
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

        private Dictionary<string, FiltreElement> buildFiltreDashboard()
        {
            Dictionary<string, FiltreElement> filterItems = new Dictionary<string, FiltreElement>();

            //string yearValue = string.Empty;
            //string choixTpsValue = string.Empty;
            //string sexeValue = string.Empty;


            //string choixTps = string.Empty;


            //if (this.RbAnneeY) yearValue = this.AnneeY;
            //if (this.rbAnneeY_1) yearValue = this.AnneeY_1;
            //if (this.rbAnneeY_2) yearValue = this.AnneeY_2;
            //if (this.rbAnneeY_3) yearValue = this.AnneeY_3;
            //if (this.rbAnneeY_4) yearValue = this.AnneeY_4;

            //if (this.rbChoixSemester)
            //{
            //    choixTps = "semestre";
            //    if (this.rbSemester_1) choixTpsValue = "1";
            //    if (this.rbSemester_2) choixTpsValue = "2";
            //}

            //else if (this.rbChoixQuater)
            //{
            //    choixTps = "trimestre";
            //    if (this.rbQ_1) choixTpsValue = "1";
            //    if (this.rbQ_2) choixTpsValue = "2";
            //    if (this.rbQ_3) choixTpsValue = "3";
            //    if (this.rbQ_4) choixTpsValue = "4";
            //}

            //else // (this.rbChoixMois)
            //{
            //    choixTps = "mois";
            //    if (this.rbJan) choixTpsValue = "&[1]&[1]&[January]";
            //    if (this.rbFev) choixTpsValue = "&[1]&[1]&[February]";
            //    if (this.rbMar) choixTpsValue = "&[1]&[1]&[March]";
            //    if (this.rbAvr) choixTpsValue = "&[1]&[2]&[April]";
            //    if (this.rbMai) choixTpsValue = "&[1]&[2]&[May]";
            //    if (this.rbJui) choixTpsValue = "&[1]&[2]&[June]";
            //    if (this.rbJul) choixTpsValue = "&[2]&[3]&[July]";
            //    if (this.rbAou) choixTpsValue = "&[2]&[3]&[August]";
            //    if (this.rbSept) choixTpsValue = "&[2]&[3]&[September]";
            //    if (this.rbOct) choixTpsValue = "&[2]&[4]&[October]";
            //    if (this.rbNov) choixTpsValue = "&[2]&[4]&[November]";
            //    if (this.rbDec) choixTpsValue = "&[2]&[4]&[December]";
            //}

            //if ((this.rbHomme) && !(this.rbFemme)) sexeValue = "Masculin";
            //else if (!(this.rbHomme) && (this.rbFemme)) sexeValue = "Feminin";
            //else sexeValue = "all";


            filterItems.Add("annee", new FiltreElement { Nom = "annee", Valeur = "2016" });
            filterItems.Add("mois", new FiltreElement { Nom = "mois", Valeur = "&[1]&[1]&[February]" });
            //filterItems.Add("entiteAdmin", new FiltreElement { Nom = "entiteAdmin", Valeur = filterList(FiltreEntiteAdmin) });
            //filterItems.Add("entiteAdmin", new FiltreElement { Nom = "entiteAdmin", Valeur = LibEntiteAdminItems });
            //filterItems.Add("trancheAge", new FiltreElement { Nom = "trancheAge", Valeur = filterList(FiltreTrancheAge) });
            //filterItems.Add("trancheAnciennete", new FiltreElement { Nom = "trancheAnciennete", Valeur = filterList(FiltreTrancheAnciennete) });
            //filterItems.Add("sexe", new FiltreElement { Nom = "sexe", Valeur = sexeValue });

            return filterItems;
        }
    }
}
