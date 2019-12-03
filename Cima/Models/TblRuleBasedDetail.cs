using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblRuleBasedDetail", Schema = "dbo")]
    public partial class TblRuleBasedDetail
    {
        [Key]
        [Column("ID_RuleBasedDetail")]
        public int IdRuleBasedDetail { get; set; }
        public string Result { get; set; }
        public string State { get; set; }
        public DateTime DateExec { get; set; }
        public string Issues { get; set; }
        [Column("ID_Action")]
        public int? IdAction { get; set; }
        [Column("ID_RuleBasedLog")]
        public int? IdRuleBasedLog { get; set; }

        public virtual TblAction IdActionNavigation { get; set; }
        public virtual TblRuleBasedLog IdRuleBasedLogNavigation { get; set; }
    }
}
