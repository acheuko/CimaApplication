using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cima.Models
{
    [Table("tblDataColumn", Schema = "dbo")]
    public partial class TblDataColumn
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Column("ID_DataColumn")]
        public int IdDataColumn { get; set; }
        [Column("DataColumn_ID")]
        public string DataColumnId { get; set; }
        public string Name { get; set; }
        public string Datatype { get; set; }
        public string Valuerange { get; set; }
        public int NumColumn { get; set; }
        //[ForeignKey("ID_Report")]
        [Column("ID_Report")]
        [UIHint("ReportColumnId")]
        public int? IdReport { get; set; }

        public virtual TblReport IdReportNavigation { get; set; }
    }
}
