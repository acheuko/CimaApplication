using Cima.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Cima.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        private readonly string ReportServerUrl = ConfigurationManager.AppSettings["ReportServerUrl"].ToString();

        public ActionResult Report(string paramUrl)
        {
            string reportFolder = ConfigurationManager.AppSettings[paramUrl].ToString();

            #pragma warning disable IDE0067 // Dispose objects before losing scope
            ViewBag.ReportViewer = GetReportViewer(reportFolder);
            #pragma warning restore IDE0067 // Dispose objects before losing scope

            return View();
        }


        public ReportViewer GetReportViewer(string reportFolder)
        {
            try
            {
                ReportViewer rptViewer = new ReportViewer
                {
                    // ProcessingMode will be Either Remote or Local  
                    ProcessingMode = ProcessingMode.Remote,
                    SizeToReportContent = true,
                    ZoomMode = ZoomMode.PageWidth,
                    Width = Unit.Percentage(100),
                    Height = Unit.Percentage(100),
                    AsyncRendering = true
                };
                rptViewer.ServerReport.ReportServerUrl = new Uri(uriString: ReportServerUrl);

                rptViewer.ServerReport.ReportPath = reportFolder;
               // rptViewer.ServerReport.ReportServerCredentials = new C();

                rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials("xxxxxxx", "xxxxxxxx","xxxxxxxx");
                //rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials("Tadjo", "Takianpi1", "TADJO-PC");

                List<ReportParameter> param = new List<ReportParameter>
                {
                    new ReportParameter("strCompany", Session["Company"].ToString())
                };

                rptViewer.ServerReport.SetParameters(param);

                rptViewer.ServerReport.Refresh();

                return rptViewer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
           
        }

    }
}
