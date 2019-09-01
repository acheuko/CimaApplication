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

        public ActionResult Report()
        {
            string reportFolder = ConfigurationManager.AppSettings["ReportPath"].ToString();

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
            rptViewer.ServerReport.ReportServerUrl = new Uri("http://localhost/ReportServer/");

            rptViewer.ServerReport.ReportPath = reportFolder;
                //String.Format("{0}/{1}",reportFolder,"Report1.rdl");
                //this.SetReportPath();

            ViewBag.ReportViewer = rptViewer;
            return View();
        }


    }
}
