using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblRule", Schema = "dbo")]
    public class TblRule
    {
        public TblRule()
        {
            //TblAction = new HashSet<TblAction>();
            //TblRuleReport = new HashSet<TblRuleReport>();
        }
        [Key]
        [Column("Id_Rule")]
        public int IdRule { get; set; }
        [Column("Rule_ID")]
        public string RuleId { get; set; }
        public string LibRule { get; set; }
        public string TypeRule { get; set; }
        public string RangeRule { get; set; }
        public string Exercice { get; set; }

        //public virtual ICollection<TblAction> TblAction { get; set; }
        //public virtual ICollection<TblRuleReport> TblRuleReport { get; set; }
    }
}
