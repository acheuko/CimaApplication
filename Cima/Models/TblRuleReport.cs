using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblRuleReport", Schema = "dbo")]
    public partial class TblRuleReport
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        public bool Active { get; set; }
        //[ForeignKey("ID_Rule")]
        [Column("ID_Rule")]
        public int? IdRule { get; set; }
        //[ForeignKey("ID_Report")]
        [Column("ID_Report")]
        public int? IdReport { get; set; }

        public virtual TblReport IdReportNavigation { get; set; }
        public virtual TblRule IdRuleNavigation { get; set; }
    }
}
