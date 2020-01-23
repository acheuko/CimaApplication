using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cima.Models;
using Cima.AppContext;
using Cima.Repository;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cima.Repository.Shared;
using Newtonsoft.Json;
using Cima.RulesBased;

namespace Cima.Controllers
{
    public class RuleBasedController : Controller
    {
        private readonly REPO_Rule repoRule;
        private readonly REPO_Report repoReport;
        private readonly REPO_Action repoaction;
        private readonly REPO_DataColumn repodatacolumn;
        private readonly REPO_RuleReport repoRuleReport;
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        SysmanDbContext db = new SysmanDbContext();

        public RuleBasedController()
        {
            repoRule = (REPO_Rule)unitOfWork.RuleRepository;
            repoReport = (REPO_Report)unitOfWork.ReportRepository;
            repoaction = (REPO_Action)unitOfWork.ActionRepository;
            repodatacolumn = (REPO_DataColumn)unitOfWork.DataColumnRepository;
            repoRuleReport = (REPO_RuleReport)unitOfWork.RuleReportRepository;
        }

        //
        // GET: /RuleBased/

        public ActionResult Index()
        {
            //var rules = repoRule.GetRules("");
            //var rr = rules.ToList();
            return View();
        }

        public ActionResult Report()
        {
            return View("Report");
        }

        public ActionResult Action()
        {
            PopulateRuleDropDownList();
            return View("Action");
        }

        public ActionResult DataColumn()
        {
            PopulateReportDropDownList();
            return View("DataColumn");
        }

        public ActionResult CreateActionRule()
        {
            return View("CreateActionView");
        }

        public ActionResult CreateRuleAction()
        {
            return View("CreateRuleView");
        }

        public ActionResult CreateReportView()
        {
            return View("CreateReportView");
        }

        public ActionResult CreateDataColumnView()
        {
            return View("CreateDataColumnView");
        }

        //
        // GET: /RuleBased/Details/5

        public ActionResult Details(int id = 0)
        {
            TblRule tblrule = db.TblRules.Find(id);
            if (tblrule == null)
            {
                return HttpNotFound();
            }
            return View(tblrule);
        }

        //
        // GET: /RuleBased/Create

        public ActionResult Create()
        {
            return View();
        }

        
        public ActionResult testdata()
        {
            DataPreparation dp = new DataPreparation();
            dp.executeProcessing("C:\\Files\\Pfn\\Tmp", "CMDOMAS005");
            return View("Index");
        }
        //
        // POST: /RuleBased/Create

        [HttpPost]
        public ActionResult Create(TblRule tblrule)
        {
            if (ModelState.IsValid)
            {
                db.TblRules.Add(tblrule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblrule);
        }

        public ActionResult SaveRules(string RuleId, string LibRule, string RangeRule, string[] Idreport, string typerule, string execice)
        {
            TblRule tblrule = new TblRule();
            int IdRule = 0;
            string libelle = "";
            if (Idreport != null)
            {
                tblrule.RuleId = RuleId;
                tblrule.LibRule = LibRule;
                tblrule.RangeRule = RangeRule;
                tblrule.Exercice = execice;
                tblrule.TypeRule = typerule;

                try
                {
                    repoRule.Insert(tblrule);
                    unitOfWork.Save();
                    IdRule = tblrule.IdRule;
                    libelle = tblrule.LibRule;
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }

                foreach (var idr in Idreport)
                {
                    TblRuleReport tblrulereport = new TblRuleReport();
                    var IDr = Int32.Parse(idr.ToString());
                    tblrulereport.IdReport = IDr;
                    tblrulereport.IdRule = IdRule;
					tblrulereport.Active = true;
                    try
                    {
                        repoRuleReport.Insert(tblrulereport);
                        unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
                    }

                }

            }

            return Json(new { success = true, data = libelle }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveReport(string ReportId, string LibReport, string DetailReport, int Nbcolumn, string Periodicite, DateTime DateLimite)
        {
            TblReport tblreport = new TblReport();
            string libelle = "";
            if (!String.IsNullOrEmpty(ReportId))
            {
                tblreport.ReportId = ReportId;
                tblreport.LibReport = LibReport;
                tblreport.Nbcolumn = Nbcolumn;
                tblreport.DetailReport = DetailReport;
                tblreport.Periodicite = Periodicite;
                tblreport.DateLimite = DateLimite;

                try
                {
                    repoReport.Insert(tblreport);
                    unitOfWork.Save();
                    libelle = tblreport.LibReport;
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { success = true, data = libelle }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDataColumn(string  DataColumnId, string Name, string Valuerange, int NumColumn, string Datatype, int IdReport)
        {
            TblDataColumn tbldatacolumn = new TblDataColumn();
            string libelle = "";
            if (!String.IsNullOrEmpty(DataColumnId))
            {
                tbldatacolumn.DataColumnId = DataColumnId;
                tbldatacolumn.Name = Name;
                tbldatacolumn.Valuerange = Valuerange;
                tbldatacolumn.NumColumn = NumColumn;
                tbldatacolumn.Datatype = Datatype;
                tbldatacolumn.IdReport = IdReport;

                try
                {
                    repodatacolumn.Insert(tbldatacolumn);
                    unitOfWork.Save();
                    libelle = tbldatacolumn.Name;
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { success = true, data = libelle }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /RuleBased/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TblRule tblrule = db.TblRules.Find(id);
            if (tblrule == null)
            {
                return HttpNotFound();
            }
            return View(tblrule);
        }

        //
        // POST: /RuleBased/Edit/5

        [HttpPost]
        public ActionResult Edit(TblRule tblrule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblrule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblrule);
        }

        //
        // GET: /RuleBased/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TblRule tblrule = db.TblRules.Find(id);
            if (tblrule == null)
            {
                return HttpNotFound();
            }
            return View(tblrule);
        }

        //
        // POST: /RuleBased/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TblRule tblrule = db.TblRules.Find(id);
            db.TblRules.Remove(tblrule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete_Rule")]
        public ActionResult Delete_Rule([DataSourceRequest] DataSourceRequest request, Models.TblRule tblrule)
        {
            try
            {
                if (tblrule != null)
                    repoRule.Delete(tblrule);
                unitOfWork.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save delete changes. Try again, and if the problem persists, see your system administrator.");
            }

            return Json("".ToDataSourceResult(request, ModelState));
        }

        [HttpPost, ActionName("Delete_Report")]
        public ActionResult Delete_Report([DataSourceRequest] DataSourceRequest request, Models.TblReport tblreport)
        {
            try
            {
                if (tblreport != null)
                    repoReport.Delete(tblreport);
                unitOfWork.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save delete changes. Try again, and if the problem persists, see your system administrator.");
            }

            return Json("".ToDataSourceResult(request, ModelState));
        }

        [HttpPost, ActionName("Delete_ActionRule")]
        public ActionResult Delete_ActionRule([DataSourceRequest] DataSourceRequest request, Models.TblAction tblaction)
        {
            try
            {
                if (tblaction != null)
                    repoaction.Delete(tblaction);
                unitOfWork.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save delete changes. Try again, and if the problem persists, see your system administrator.");
            }

            return Json("".ToDataSourceResult(request, ModelState));
        }

        [HttpPost, ActionName("Delete_DataColumn")]
        public ActionResult Delete_DataColumn([DataSourceRequest] DataSourceRequest request, Models.TblDataColumn tbldatacolumn)
        {
            try
            {
                if (tbldatacolumn != null)
                    repodatacolumn.Delete(tbldatacolumn);
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
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateRuleDropDownList(object selectedMenu = null)
        {
            var rulesQuery = repoRule.GetAll(
                orderBy: q => q.OrderBy(d => d.IdRule));
            ViewBag.Rules = rulesQuery;
        }

        public ActionResult Rule_Read([DataSourceRequest]DataSourceRequest request)
        {
            var rules = repoRule.GetAll();
            return Json(rules.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Rule_GetAll()
        {
            var Rules = repoRule.GetAll();
            return Json(Rules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_Read([DataSourceRequest]DataSourceRequest request)
        {
            var reports = repoReport.GetAll();

            var listreport = reports.ToList().ToDataSourceResult(request, r => new TblReport 
            { 
                IdReport = r.IdReport,
                ReportId = r.ReportId,
                LibReport = r.LibReport,
                DetailReport = r.DetailReport,
                Nbcolumn = r.Nbcolumn,
                Periodicite = r.Periodicite,
                DateLimite = r.DateLimite
            });

            return Json(listreport, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_GetAll(string ruleid = "")
        {
            IEnumerable<TblReport> Reports = new List<TblReport>();
            List<TblReport> listreports = new List<TblReport>();
            if (!String.IsNullOrEmpty(ruleid))
            {
                var ruleId = int.Parse(ruleid);
                var rulereports = (repoRuleReport.GetAll()).Where(rr => rr.IdRule == ruleId).ToList();
                foreach (var rr in rulereports)
                {
                    var report = (repoReport.GetAll()).Where(r => r.IdReport == rr.IdReport).FirstOrDefault();
                    var rpt = new TblReport
                    {
                        IdReport = report.IdReport,
                        ReportId = report.ReportId,
                        LibReport = report.LibReport,
                        DetailReport = report.DetailReport,
                        Nbcolumn = report.Nbcolumn,
                        Periodicite = report.Periodicite,
                        DateLimite = report.DateLimite
                    };
                    listreports.Add(rpt);
                }
                Reports = listreports;

            }
            else {
                var _Reports = repoReport.GetAll();

                foreach (var report in _Reports)
                {
                    var rpt = new TblReport
                    {
                        IdReport = report.IdReport,
                        ReportId = report.ReportId,
                        LibReport = report.LibReport,
                        DetailReport = report.DetailReport,
                        Nbcolumn = report.Nbcolumn,
                        Periodicite = report.Periodicite,
                        DateLimite = report.DateLimite
                    };
                    listreports.Add(rpt);
                }
                Reports = listreports;
            } 
            return Json(Reports, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataColumn_GetAll(string reportid = "")
        {
            IEnumerable<TblDataColumn> DataColumns = new List<TblDataColumn>();
            if (!String.IsNullOrEmpty(reportid))
            {
                List<TblDataColumn> _DataColumns = new List<TblDataColumn>();
                var reportId = int.Parse(reportid);
                DataColumns = (repodatacolumn.GetAll()).Where(dc => dc.IdReport == reportId).ToList();
                foreach (var dcol in DataColumns)
                {
                    var column = new TblDataColumn
                                {

                                    IdDataColumn = dcol.IdDataColumn,
                                    DataColumnId = dcol.DataColumnId,
                                    Name = dcol.Name,
                                    Datatype = dcol.Datatype,
                                    Valuerange = dcol.Valuerange,
                                    NumColumn = dcol.NumColumn,
                                    IdReport = dcol.IdReport
                                };
                    _DataColumns.Add(column);
                }
                DataColumns = _DataColumns;
            }
            else DataColumns = repodatacolumn.GetAll();
            return Json(DataColumns, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Action_Read([DataSourceRequest]DataSourceRequest request)
        {
            var actions = repoaction.GetAll();

            var listaction = actions.ToList().ToDataSourceResult(request, a => new TblAction
            {
                ActionId = a.ActionId,
                LibAction = a.LibAction,
                DetailAction = a.DetailAction,
                Statement = a.Statement,
                Accurency = a.Accurency,
                Treshold = a.Treshold,
                Operator = a.Operator,
                Complexaction = a.Complexaction,
                IdRule = a.IdRule
            });

            return Json(listaction, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataColumn_Read([DataSourceRequest]DataSourceRequest request)
        {
            var datacolumns = repodatacolumn.GetAll();

            var listdc = datacolumns.ToList().ToDataSourceResult(request, dc => new TblDataColumn
            {
                
                IdDataColumn = dc.IdDataColumn,
                DataColumnId = dc.DataColumnId,
                Name = dc.Name,
                Datatype = dc.Datatype,
                Valuerange = dc.Valuerange,
                NumColumn = dc.NumColumn,
                IdReport = dc.IdReport
            });

            return Json(listdc, JsonRequestBehavior.AllowGet);
        }

        private void PopulateReportDropDownList(object selectedMenu = null)
        {
            var ReportQuery = repoReport.GetAll(
                orderBy: q => q.OrderBy(d => d.IdReport));
            ViewBag.Report = ReportQuery;
        }

        public ActionResult SaveAction(string ActionId, string LibAction, string Statement, string DetailAction, int IdRule, string Operator, int Accurency = 0, int Treshold = 0)
        {
            TblAction tblaction = new TblAction();
            string libelle = "";
            if (IdRule > 0)
            {
                tblaction.ActionId = ActionId;
                tblaction.LibAction = LibAction;
                tblaction.DetailAction = DetailAction;
                tblaction.Statement = Statement;
                tblaction.Operator = Operator;
                tblaction.Accurency = Accurency;
                tblaction.Treshold = Treshold;
                tblaction.Complexaction = "";
                try
                {
                    repoaction.Insert(tblaction);
                    unitOfWork.Save();
                    libelle = tblaction.LibAction;
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { success = true, data = libelle }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TblReport_Update([DataSourceRequest] DataSourceRequest request,
             Models.TblReport report)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    report.ReportId = report.ReportId ?? "";
                    report.LibReport = report.LibReport ?? "";
                    repoReport.Update(report);
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
    }
}