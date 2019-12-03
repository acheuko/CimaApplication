using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cima.Models
{
    [Table("tblReport", Schema = "dbo")]
    public partial class TblReport
    {
        public TblReport()
        {
            TblDataColumn = new HashSet<TblDataColumn>();
            TblRuleReport = new HashSet<TblRuleReport>();
        }
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Column("ID_Report")]
        public int IdReport { get; set; }
        [Column("Report_ID")]
        public string ReportId { get; set; }
        public string LibReport { get; set; }
        public string DetailReport { get; set; }
        public int Nbcolumn { get; set; }
        public string Periodicite { get; set; }
        [DisplayName("Date limite")]
        public DateTime DateLimite { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblDataColumn> TblDataColumn { get; set; }
        [JsonIgnore] 
        public virtual ICollection<TblRuleReport> TblRuleReport { get; set; }
    }
}
