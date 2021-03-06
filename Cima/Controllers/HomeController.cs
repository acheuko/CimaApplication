﻿using System;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Cima.Repository.TestData;
using Cima.Models.TestModel;
using Cima.Models.Shared;
using System.Collections.Generic;
using Cima.Helpers;
using iText.Html2pdf;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;

namespace Cima.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        //public static final String TARGET = "target/results/ch01/";
        //public static final String DEST = String.format("%stest-03.pdf", TARGET);

        private FiltreDashboard filtre;

        public ActionResult Login()
        {
           // ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }

        [Authorize(Roles = Profil.ADMIN)]
        public ActionResult Index()
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

        [Authorize(Roles = Profil.ADMIN)]
        public ActionResult Dashboard2()
        {
            ViewBag.Message = "Your app description page.";


            return View();
        }

        [Authorize(Roles = Profil.ADMIN)]
        public ActionResult Dashboard3()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportPDF(string fileContents)
        {
            //new FileStream("C:\\Files\\test.pdf", FileMode.Create, FileAccess.Write)
            using (var stream = new MemoryStream())
            {

                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.Add(new Paragraph("Goodbye!"));
                document = HtmlConverter.ConvertToDocument(fileContents, writer);
                
                
                document.Close();


                return File(stream.ToArray(), "application/pdf", "evolMonant.pdf");

            }

        }

        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            //List<Technology> technologies = StaticData.Technologies;

            List<GridDetails> gridDetails = getGridDetails();

            string[] columns = { "CallCategory", "BSC_LN", "StartTime", "PhoneNumber", "Country", "Origin", "Duration" };

            byte[] filecontent = ExcelExportHelper.ExportExcel(gridDetails, "Grid Details", true, columns);

            return File(filecontent, ExcelExportHelper.ExcelContentType, "TestData.xlsx");
        }

        [HttpGet]
        public FileContentResult ExportToCsv()
        {
            //List<Technology> technologies = StaticData.Technologies;

            List<GridDetails> gridDetails = getGridDetails();

            string csv = CsvExportHelper.ListToCSV(gridDetails);

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "testData.csv");
        }

        public JsonResult Country_Read()
        {

            //filtre = new FiltreDashboard();

            //this.filtre.DicoFiltres = this.buildFiltreDashboard();

            //Dictionary<string, ObservableCollection<BusinessModel>> criteriaItems = new REPO_CriteriaItems().GetFilterData(filtre);
            //ObservableCollection<BusinessModel> catSocioPro = new ObservableCollection<BusinessModel>();

            //catSocioPro = criteriaItems["categorieSocioPro"];

            //return Json(catSocioPro, JsonRequestBehavior.AllowGet);

            List<string> catSocioPro = new List<string>
            {
                "Nigeria",
                "Congo",
                "Bénin",
                "Cameroon",
                "RCA",
                "Ghana"
            };

            return Json(catSocioPro, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Network_Read()
        {
            List<string> networks = new List<string>
            {
                "BJNVIAS002",
                "BJNVIAS003"
            };

            return Json(networks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CallType_Read()
        {
            List<string> callTypes = new List<string>
            {
                "Q12019",
                "Q22019",
                "Q32019",
                "Q12018"
            };

            return Json(callTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TableDetails_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<GridDetails> gridDetails = getGridDetails();
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

        private List<GridDetails> getGridDetails()
        {
            return new List<GridDetails>
            {
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15, 00, 00, 32), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Abaya", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" },
                new GridDetails { CallCategory = "Outgoing", BSC_LN = "Udeni", StartTime = new DateTime(2006, 6, 15), PhoneNumber = " 234-80-91054665 ", Country = "Nigeria - Mobile - Other", Origin = "2348183833424", Duration = "00:10:14" }
            };
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
