using Deedle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cima.Models;
using System.Text.RegularExpressions;
using Cima.AppContext;
using Cima.Repository;

namespace Cima.RulesBased
{
    public interface DataPreProcessor
    {
        //
        // Résumé :
        //     Chargement des fichiers du dossier temporaire  à partir du code de la companie
        //
        // Retourne :
        //     La liste des fichiers de l'entreprise
        IEnumerable<string> ListFichiersEntreprise(string tmpPath, string company);
        //
        // Résumé :
        //     Chargement des frameset de tout les  types de fichiers de la companie
        //
        // Retourne :
        //     
        void LoadFiles(string tmpPath, string company);
        //
        // Résumé :
        //     Appliquer une méthode à une règle de cohérence
        void applyMethodFromRuleCoherence(List<TblReport> lreport, TblRule ruleset);
        //
        // Résumé :
        //     Appliquer une méthode à une règle de conformité
        void applyMethodFromRuleConformite(TblReport report, TblRule ruleset);
        //
        // Résumé :
        //     Vérification des instances de fichiers déjà chargées
        //
        // Retourne :
        //     La liste des fichiers de chargés
        List<string> getListOflaodedfiles();
        //
        // Résumé :
        //     Extrait la liste des rapports reférencés dans une règle de cohérence
        //
        // Retourne :
        //     La liste des rapport reférencés par la règle
        List<TblReport> getReferenceReport(TblRule ruleset);
        //
        // Résumé :
        //     Récupère dynamiquement la liste des instances dataframe des fichiers de gestion
        //
        // Retourne :
        //     L'instance du fichier de gestion
        Dictionary<string, Frame<int, string>> getIntanceFromRule(TblRule ruleset);
        //
        // Résumé :
        //     Récupère dynamiquement l'instance dataframe d'un fichier de gestion
        //
        // Retourne :
        //     L'instance du fichier de gestion
        List<TblRule> getListOfRules(int reportid);
        //
        // Résumé :
        //     Récupère la liste des actions d'une règle 
        //
        List<TblAction> getListOfActions(int codeRule);
        //
        // Résumé :
        //     Récupère dynamiquement l'instance dataframe d'un fichier de gestion
        //
        // Retourne :
        //     L'instance du fichier de gestion
        List<TblReport> getListOfReports(List<string> masks);
        //
        // Résumé :
        //     Récupère la liste des colonnes d'un rapport
        //
        // Retourne :
        //     La liste des colonnes d'un rapport
        List<TblDataColumn> getListOfDataColumn(int repotid);
    }

    public interface DataManipulation
    {
        //
        // Résumé :
        //     compare l'écart entre 2 nombres en fonction du seuil et de la précision
        //
        // Retourne :
        //     La différence
        float diff(int nb1, int nb2, float treshold, float accurency);
        //
        // Résumé :
        //     compare l'écart entre 2 chaines de caractère
        bool checkIsSame(TblDataColumn datacolumn, Frame<int, string> df);
        //
        // Résumé :
        //     Vérifie le type de données de la colonne
        bool checkType(TblDataColumn datacolumn, Frame<int, string> df);
        //
        // Résumé :
        //     Vérifie l'intervalle de valeurs possible
        bool checkRangeData(TblDataColumn datacolumn);
        //
        // Résumé :
        //     Vérifie l'exhaustivité de la liste des colonnes
        bool checkNunberColumnsOfReport(TblReport report, Frame<int, string> df);
        //
        // Résumé :
        //     Vérifie l'ordre de la colonne
        bool checkOrderColumnsOfReport(TblDataColumn dc, Frame<int, string> df);
    }
    public class DataPreparation : DataPreProcessor, DataManipulation
    {
        #region Déclaration des variables
        public static Frame<int, string> dframe_c4s { get; set; }
        public static Frame<int, string> dframe_g15 { get; set; }
        public static Frame<int, string> dframe_g16 { get; set; }
        public static Frame<int, string> dframe_c4 { get; set; }
        public static Frame<int, string> dframe_t1 { get; set; }
        public static Frame<int, string> dframe_rs2vie { get; set; }
        public static Frame<int, string> dframe_g11 { get; set; }
        public static Frame<int, string> dframe_c25vie { get; set; }
        public static Frame<int, string> dframe_g10 { get; set; }
        public static Frame<int, string> dframe_c20c21vie { get; set; }
        public static Frame<int, string> dframe_c25bistabavie { get; set; }
        public static Frame<int, string> dframe_c26vie { get; set; }
        public static Frame<int, string> dframe_g1g4 { get; set; }
        public static Frame<int, string> dframe_ra1 { get; set; }
        public static Frame<int, string> dframe_ra2 { get; set; }
        public static Frame<int, string> dframe_t2 { get; set; }
        public static Frame<int, string> dframe_rs1 { get; set; }
        public static Frame<int, string> dframe_rs2iard { get; set; }


        private static Dictionary<int, bool> alreadyCheck = new Dictionary<int, bool>();
        static Dictionary<string, string> listmask = new Dictionary<string, string>();
        static Dictionary<string, string> listDetailsLog = new Dictionary<string, string>();
        static Dictionary<string, Frame<int, string>> lisInstanceDF = new Dictionary<string, Frame<int, string>>();
        private SysmanDbContext contextDB;
        private static string CptSequence = "";
        private static string tmpPath = "";
        private static string strCompany = "";
        private static string localIssues = "None";
        private static TblRuleBasedLog tblrulebasedlog;
        #endregion

        public DataPreparation()
        {
            
        }

        public DataPreparation(SysmanDbContext sc)
        {
            contextDB = sc;
        }
        public DataPreparation(SysmanDbContext sc, string _tmpPath, string _strCompany)
        {
            contextDB = sc;
            tmpPath = _tmpPath;
            strCompany = _strCompany;
            CptSequence = (new Random()).Next(0, 1000000000).ToString() + strCompany;
            listmask = new Dictionary<string, string>();
            tblrulebasedlog = new TblRuleBasedLog(CptSequence, "Contrôle de cohérence de l'upload " + CptSequence + " du " + DateTime.Now.Date.ToString(), false, DateTime.Now, DateTime.Now);
            contextDB.tblrulebasedlog.Add(tblrulebasedlog);
            contextDB.tblrulebasedlog.Add(tblrulebasedlog);
            contextDB.SaveChanges();
        }

        public IEnumerable<string> ListFichiersEntreprise(string tmpPath, string company)
        {
            Collection<String> Coll = new Collection<String>();
            IEnumerable<string> files = Coll as IEnumerable<String>;
            
            try
            {
                files = from file in
                Directory.EnumerateFiles(tmpPath, "*.PFN*", SearchOption.TopDirectoryOnly)
                        where file.Contains(company)
                        select file;
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
            return files;
        }

        public void LoadFiles(string tmpPath, string company)
        {
            var listeFiles = this.ListFichiersEntreprise(tmpPath, company);

            foreach (var file in listeFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string mask = fileName.Split('_')[3];
                switch (mask)
                {
                    case ("C4S"):
                        try
                        {
                            dframe_c4s = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c4s = null;
                        }
                        break;
                    case ("G15"):
                        try
                        {
                            dframe_g15 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c4s = null;
                        }
                        break;
                    case ("G16"):
                        try
                        {
                            dframe_g16 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c4s = null;
                        }
                        break;
                    case ("C4"):
                        try
                        {
                            dframe_c4 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c4 = null;
                        }
                        break;
                    case ("T1"):
                        try
                        {
                            dframe_t1 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_t1 = null;
                        }
                        break;
                    case ("RS2VIE"):
                        try
                        {
                            dframe_rs2vie = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_rs2vie = null;
                        }
                        break;
                    case ("G11"):
                        try
                        {
                            dframe_g11 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_g11 = null;
                        }
                        break;
                    case ("C25VIE"):
                        try
                        {
                            dframe_c25vie = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c25vie = null;
                        }
                        break;
                    case ("G10"):
                        try
                        {
                            dframe_g10 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_g10 = null;
                        }
                        break;
                    case ("C20C21Vie"):
                        try
                        {
                            dframe_c20c21vie = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c20c21vie = null;
                        }
                        break;
                    case ("C25BisTabAVIE"):
                        try
                        {
                            dframe_c25bistabavie = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c25bistabavie = null;
                        }
                        break;
                    case ("C26VIE"):
                        try
                        {
                            dframe_c26vie = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_c26vie = null;
                        }
                        break;
                    case ("G1G4"):
                        try
                        {
                            dframe_g1g4 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_g1g4 = null;
                        }
                        break;
                    case ("RA1"):
                        try
                        {
                            dframe_ra1 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_ra1 = null;
                        }
                        break;
                    case ("RA2"):
                        try
                        {
                            dframe_ra2 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_ra2 = null;
                        }
                        break;
                    case ("T2"):
                        try
                        {
                            dframe_t2 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_t2 = null;
                        }
                        break;
                    case ("RS1"):
                        try
                        {
                            dframe_rs1 = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_rs1 = null;
                        }
                        break;
                    case ("RS2IARD"):
                        try
                        {
                            dframe_rs2iard = Frame.ReadCsv(Path.Combine(tmpPath, file), true, true, 0, "", "|");
                        }
                        catch (Exception)
                        {
                            dframe_rs2iard = null;
                        }
                        break;
                }
            }
        }

        // Appliquer une méthode à une règle de cohérence
        public void applyMethodFromRuleCoherence(List<TblReport> lreport, TblRule ruleset)
        {
            var actions = this.getListOfActions(ruleset.IdRule);
            foreach (var a in actions)
            {
                var data = DataPreparation.dynamicPatternCoherence(a, ruleset);
                var detlog = new TblRuleBasedDetail();
                detlog.IdRuleBasedLog = tblrulebasedlog.IdRuleBasedLog;
                detlog.Result = data;
                detlog.IdAction = a.IdAction;
                detlog.DateExec = DateTime.Now;
                detlog.Issues = (!String.IsNullOrEmpty(localIssues)) ? localIssues : "None";
                detlog.State = DetermineStateAction(data);
                contextDB.tblrulebaseddetail.Add(detlog);
                contextDB.SaveChanges();
                listDetailsLog.Add(a.ActionId, detlog.State);
                localIssues = "None";
            }
        }

        // Appliquer une méthode à une règle de conformité
        public void applyMethodFromRuleConformite(TblReport report, TblRule ruleset)
        {
            var actions = this.getListOfActions(ruleset.IdRule);
            foreach (var a in actions)
            {
                var data = DataPreparation.dynamicPatternConformite(a.Operator, a.Statement);
                var detlog = new TblRuleBasedDetail();
                detlog.IdRuleBasedLog = tblrulebasedlog.IdRuleBasedLog;
                detlog.Result = data;
                detlog.IdAction = a.IdAction;
                detlog.DateExec = DateTime.Now;
                detlog.Issues = (!String.IsNullOrEmpty(localIssues)) ? localIssues : "None";
                detlog.State = DetermineStateAction(data);
                contextDB.tblrulebaseddetail.Add(detlog);
                contextDB.SaveChanges();
                listDetailsLog.Add(a.ActionId, detlog.State);
                localIssues = "None";
            }
        }

        public string DetermineStateAction(string data)
        {
            float flotant;
            bool booleen;
            if (float.TryParse(data, out flotant))
            {
                if (flotant >= 0) return "success";
                else return "falliure";
            }
            else if (Boolean.TryParse(data, out booleen))
            {
                if (booleen) return "success";
                else return "falliure";
            }
            else
            {
                // string case
                if (!string.IsNullOrEmpty(data)) {
                    if (data.Equals("Règle non adaptée")) return "ignore";
                    else return "success";
                }
                else return "falliure";
            }
        }

        public List<string> getListOflaodedfiles()
        {
            return new List<string>();
        }

        public List<TblReport> getReferenceReport(TblRule ruleset)
        {
            List<TblReport> lreport = new List<TblReport>();
            var reportrules = contextDB.TblRuleReport
                  .Where(dc => dc.IdRule == ruleset.IdRule)
                  .ToList();
            if (reportrules.Count > 0)
            {
                foreach (var rr in reportrules)
                {
                    var report = contextDB.TblReport
                       .Where(r => r.IdReport == rr.IdReport)
                       .FirstOrDefault<TblReport>();
                    if ((report != null) && (!lreport.Contains(report))) lreport.Add(report);
                }
            }
            return lreport;
        }

        public Dictionary<string, Frame<int, string>> getIntanceFromRule(TblRule ruleset)
        {
            return new Dictionary<string, Frame<int, string>>();
        }

        public List<TblRule> getListOfRules(int reportid)
        {
            List<TblRule> lrule = new List<TblRule>();
            var reportrules = contextDB.TblRuleReport
                  .Where(dc => dc.IdReport == reportid)
                  .ToList();
            if(reportrules.Count > 0)
            {
                foreach (var rr in reportrules)
                {
                    var rule = contextDB.TblRules
                       .Where(r => r.IdRule == rr.IdRule)
                       .FirstOrDefault<TblRule>();
                    if ((rule != null) && (!lrule.Contains(rule))) lrule.Add(rule);
                }
            }
            return lrule;
        }

        public void activateRuleReport(bool actif)
        {
            var rulereport = new TblRuleReport
            {
                IdRule = 1,
                IdReport = 1,
                Active = actif
            };
            contextDB.TblRuleReport.Add(rulereport);
            contextDB.SaveChanges();
        }


        public List<TblAction> getListOfActions(int ruleid)
        {
            var actions = contextDB.TblAction
                  .Where(a => a.IdRule == ruleid)
                  .ToList();
            return actions;
        }

        public List<TblReport> getListOfReports(List<string> masks)
        {
            List<TblReport> lrpt = new List<TblReport>();
            foreach (var m in masks)
            {
                var report = contextDB.TblReport
                   .Where(r => r.ReportId == m)
                   .FirstOrDefault<TblReport>();
                if(report != null) lrpt.Add(report);
            }
            return lrpt;
        }

        public TblReport getOneReport(string mask)
        {
            var report = contextDB.TblReport
                .Where(r => r.ReportId == mask)
                .FirstOrDefault<TblReport>();
            return report;
        }

        public Series<T, ObjectSeries<string>> getOneT<T>(string data, string coldata, Frame<int,string> list)
        {
            try
            {
                var df = list.IndexRows<T>(coldata).SortRowsByKey();
                //var df1 = list.IndexRowsUsing(r => r.GetAs<string>("TypeApport") + r.GetAs<string>("Exercice"));
                var item = df.Rows.Where(kvp => kvp.Key.Equals(data));
                return item;
            }
            catch (Exception) {
                return null;
            }
            
        }

        public void getListOfMaskFromDB()
        {
            List<TblReport> lrpt = new List<TblReport>();
            var report = contextDB.TblReport.ToList();
            if (report != null)
            {
                foreach (var r in report)
                {
                    listmask.Add(r.ReportId, r.LibReport);
                    //DataPreparation DataPreparation = new DataPreparation(new SysmanDbContext());
                    DataPreparation DataPreparation = new DataPreparation();
                    foreach (var prop in DataPreparation.GetType().GetProperties())
                    {
                        if (prop.Name.Contains(("_" + r.ReportId).ToLower())) lisInstanceDF.Add(r.ReportId, (Frame<int, string>)prop.GetValue(DataPreparation));
                    }
                }
            }
        }

        public List<TblDataColumn> getListOfDataColumn(int reportid)
        {
            var datacolumns = contextDB.TblDataColumn
                  .Where(dc => dc.IdReport == reportid)
                  .ToList();
            return datacolumns;
        }

        public TblDataColumn getListOfDataColumnFrom(int reportid, string colname)
        {
            var datacolumn = contextDB.TblDataColumn
                  .Where(dc => dc.IdReport == reportid && dc.Name == colname)
                  .FirstOrDefault<TblDataColumn>();
            return datacolumn;
        }

        static void Main(string[] args)
        {
            //DataPreparation dp = new DataPreparation(new SysmanDbContext(), "C:\\Files\\Pfn\\Tmp", "CMDOMAS005");
            DataPreparation dp = new DataPreparation();
            dp.executeProcessing("C:\\Files\\Pfn\\Tmp", "CMDOMAS005");
            Console.WriteLine("Exécution terminée");
            Console.ReadLine();
        }

        public void executeProcessing(string tmpPath, string strCompany)
        {
            DataPreparation dp = new DataPreparation(new SysmanDbContext(), tmpPath, strCompany);
            // récupère la liste des fichiers d'entreprise dans le dossier temporaire
            dp.LoadFiles(tmpPath, strCompany);
            // Créé un tableau associatif des noms de rapports et de leurs instances
            dp.getListOfMaskFromDB();
            // Parcourt la liste des rapports afin d'appliquer les règles
            foreach (var report in lisInstanceDF)
            {
                // On vérifie si le fichier à été chargé sur l'espace temporaire
                if(report.Value != null)
                {
                    // On récupère la structure du rapport à analyser
                    var _report = dp.getOneReport(report.Key);
                    // On récupère la liste des règles qui concernent le rapport à analyser
                    var listRules = dp.getListOfRules(_report.IdReport);
                    // Execution de l'ensemble des règles de conformité/cohérence sur le rapport
                    foreach (var rule in listRules)
                    {
                        // Optimiser l'execution des règles pour supprimer la symétrie des associations
                        if (rule.TypeRule.Equals("coherence") && !alreadyCheck.ContainsKey(rule.IdRule))
                        {
                            dp.applyMethodFromRuleCoherence(dp.getReferenceReport(rule), rule);
                            alreadyCheck.Add(rule.IdRule, true);
                        }
                        else if (rule.TypeRule.Equals("conformite") && !alreadyCheck.ContainsKey(rule.IdRule))
                        {
                            dp.applyMethodFromRuleConformite(_report, rule);
                            alreadyCheck.Add(rule.IdRule, true);
                        }
                    }
                }
                
            }
            // Mettre à jour la date de fin et le statut de la séquence si elle est complete
            if (dp.checkIfSequenceComplete())
            {
                dp.updateStateSequence(true);
                SysmanDbContext DbCtx = new SysmanDbContext();
                var _seq = DbCtx.tblrulebasedlog.Where(l => l.Sequence == CptSequence).FirstOrDefault();
                var _logDet = DbCtx.tblrulebaseddetail.Where(ld => ld.IdRuleBasedLog == _seq.IdRuleBasedLog).ToList();
            }
        }

        static string dynamicPatternCoherence(TblAction a, TblRule rule)
        {
            //DataPreparation dprep = new DataPreparation(new SysmanDbContext());
            DataPreparation dprep = new DataPreparation();
            var tab = a.Statement.Split(new[] { a.Operator }, StringSplitOptions.None);
            string col1 = DataPreparation.extractColumnNext(tab[0]);
            string rname1 = DataPreparation.extractReportName(tab[0]);
            string col2 = DataPreparation.extractColumn(tab[1]);
            string rname2 = DataPreparation.extractReportName(tab[1]);
            //string rname2 = DataPreparation.extractReportName(tab[1].Substring(1, tab[1].Length-1));
            var report1 = DataPreparation.lisInstanceDF[rname1];
            var report2 = DataPreparation.lisInstanceDF[rname2];
            string data = "";

            if (!String.IsNullOrEmpty(col1) && !String.IsNullOrEmpty(col2) && !String.IsNullOrEmpty(rname1) && !String.IsNullOrEmpty(rname2) && (report1 != null) && (report2 != null))
            {
                switch (a.Operator)
                {
                    case ("diff"):
                        
                        var item1 = dprep.getOneT<string>(rule.Exercice, "Exercice", report1);
                        var item2 = dprep.getOneT<string>(rule.Exercice, "Exercice", report2);
                        if (item1 != null && item2 != null)
                        {
                            data = dprep.diff(Int32.Parse(item1.GetAt(0).Get(col1).ToString()), Int32.Parse(item2.GetAt(0).Get(col2).ToString()), a.Treshold, (float)a.Accurency).ToString();
                        }
                        else data = "";
                        break;
                    case ("equal"):
                        var _item1 = dprep.getOneT<string>(rule.Exercice, "Exercice", report1);
                        var _item2 = dprep.getOneT<string>(rule.Exercice, "Exercice", report2);
                        if (_item1 != null && _item2 != null)
                        {
                            data = dprep.equal(_item1.GetAt(0).Get(col1).ToString(), _item2.GetAt(0).Get(col2).ToString()).ToString();
                        }
                        else data = "";
                        break;
                }
            }
            return data;
        }

        static string dynamicPatternConformite(string _operator, string statement)
        {
            DataPreparation dprep = new DataPreparation(new SysmanDbContext());
            var tab = statement.Split(new[] { _operator }, StringSplitOptions.None);
            string data = "";
            if (!String.IsNullOrEmpty(tab[0]))
            {
                string col = DataPreparation.extractColumn(tab[0]);
                string rname = DataPreparation.extractReportName(tab[0]);
                var report = DataPreparation.lisInstanceDF[rname];
                var datacolumn = dprep.getListOfDataColumnFrom(dprep.getOneReport(rname).IdReport, col);
                if (!String.IsNullOrEmpty(col) && !String.IsNullOrEmpty(rname) && (report != null) && (datacolumn != null))
                {
                    switch (_operator)
                    {
                        case ("regex"):
                            var regex = DataPreparation.extractRegEx(tab[1]);
                            data = dprep.checkRegExpression(datacolumn, report, regex).ToString();
                            break;
                        case ("ordercol"):
                            data = dprep.checkOrderColumnsOfReport(datacolumn, report).ToString();
                            break;
                        case ("checkType"):
                            data = dprep.checkType(datacolumn, report).ToString();
                            break;
                        case ("issame"):
                            data = dprep.checkIsSame(datacolumn, report).ToString();
                            break;
                        case ("ncol"):
                            data = dprep.checkNunberColumnsOfReport(dprep.getOneReport(rname), report).ToString();
                            break;
                    }
                }
            }
            return data;
        }

        public void updateStateSequence(bool boolVal)
        {
            if (contextDB == null) contextDB = new SysmanDbContext();
            tblrulebasedlog.Complete = boolVal;
            tblrulebasedlog.DateEnd = DateTime.Now;
            //contextDB.tblrulebasedlog.Update(tblrulebasedlog);
            this.updateLog(tblrulebasedlog, boolVal);
            //contextDB.SaveChanges();
        }

        public void updateLog(TblRuleBasedLog tblrulebasedlog, bool boolVal) 
        {
            if (tblrulebasedlog.IdRuleBasedLog > 0) {
                using (contextDB)
                {
                    var statementQuery = "UPDATE dbo.TblRuleBasedLog SET Complete = '" + boolVal + "', DateEnd = '" + DateTime.Now.ToString() + "' WHERE ID_RuleBasedLog = " + tblrulebasedlog.IdRuleBasedLog.ToString();
                    int noOfRowUpdated  = contextDB.Database.ExecuteSqlCommand(statementQuery);
                }
            }
        }

        public bool checkIfSequenceComplete()
        {
            var listelts = listDetailsLog.Where(elt => elt.Value.Equals("falliure"));
            if (listelts.Count() > 0) return false;
            else return true;
        }

        public static string extractColumn(string s)
        {
            var strData = "";
            try
            {
                strData = (s.Split('(')[1]).Substring(0, s.Split('(')[1].Length - 1);
            }
            catch (Exception)
            {
                localIssues = "Règle non adaptée";
                return strData;
            }
            return strData;
        }
        public static string extractColumnNext(string s)
        {
            var strData = "";
            try
            {
                strData = (s.Split('(')[1]).Substring(0, s.Split('(')[1].Length - 2);
            }
            catch (Exception)
            {
                localIssues = "Règle non adaptée";
                return strData;
            }
            return strData;
        }
        public static string extractReportName(string s)
        {
            var strData = "";
            try
            {
                strData = s.Split('(')[0];
            }
            catch (Exception)
            {
                localIssues = "Règle non adaptée";
                return strData;
            }
            return strData;
        }
        public static string extractRegEx(string s)
        {
            var strData = "";
            try
            {
                // Sans séparateur '_' on commence à 0
                strData = s.Substring(0, s.Length - 1);
            }
            catch (Exception)
            {
                localIssues = "Règle non adaptée";
                return strData;
            }
            return strData;
        }
        public float diff(int nb1, int nb2, float treshold, float accurency)
        {
            try
            {
                var ecart = Math.Abs(nb1 - nb2);
                if (ecart > 0)
                {
                    float ecartTreshold = ecart - treshold;
                    if (ecartTreshold > 0) return ecart;
                }
            }
            catch (Exception ex)
            {
                localIssues = ex.Message;
                return -1;
            }
            return 0;
        }

        public bool equal(string v1, string v2)
        {
            var boolvar = false;
            try
            {
                if (v1.Equals(v2))
                {
                    boolvar = true;
                }
            }
            catch (Exception ex)
            {
                localIssues = ex.Message;
                return false;
            }
            return boolvar;
        }

        public bool checkIsSame(TblDataColumn datacolumn, Frame<int, string> df)
        {
            try
            {
                var serieVal = df.GetColumn<String>(datacolumn.Name);
                if (serieVal.KeyCount > 0)
                {
                    var firstval = serieVal.GetAt(0);
                    var luniq = serieVal.Select(kvp => kvp.Value).Where(kvp => kvp.Value == firstval);
                    if (luniq.KeyCount.Equals(serieVal.KeyCount)) return true;
                }
            }
            catch (Exception ex) {
                localIssues = ex.Message;
                return false; 
            }
            return false; 
        }

        public bool checkType(TblDataColumn datacolumn, Frame<int, string> df)
        {
            try
            {
                var serieVal = df.GetColumn<String>(datacolumn.Name);
                var ltypes = serieVal.Select(kvp => kvp.Value.GetType());
                var luniqtypes = ltypes.Select(kvp => kvp.Value).Where(kvp => kvp.Value.Name == datacolumn.Datatype);
                if (luniqtypes.KeyCount.Equals(ltypes.KeyCount)) return true;
                return false;
            }
            catch(Exception ex){
                localIssues = ex.Message;
                return false; 
            }
        }

        public bool checkRangeData(TblDataColumn datacolumn)
        {
            return false;
        }

        public bool checkNunberColumnsOfReport(TblReport report, Frame<int, string> df)
        {
            try
            {
                if (report.Nbcolumn.Equals(df.ColumnCount)) return true;
                else return false;
            }
            catch (Exception ex)
            {
                localIssues = ex.Message;
                return false;
            }
        }

        public bool checkOrderColumnsOfReport(TblDataColumn dc, Frame<int, string> df)
        {
            try
            {
                if (dc.NumColumn.Equals(df.GetColumn<String>(dc.Name).Index)) return true;
                else return false;
            }
            catch (Exception ex)
            {
                localIssues = ex.Message;
                return false;
            }
        }

        public bool checkRegExpression(TblDataColumn dc, Frame<int, string> df, string regex)
        {
            bool boolVal = false;
            try
            {
                var serieVal = df.GetColumn<String>(dc.Name);
                for (var i = 0; i < serieVal.ValueCount; i++)
                {
                    Match match = Regex.Match(serieVal[i], regex, RegexOptions.None);
                    if (!match.Success) boolVal = true;
                }
            }
            catch (Exception ex)
            {
                localIssues = ex.Message;
                return false;
            }
            return boolVal;
        }
    }

}
