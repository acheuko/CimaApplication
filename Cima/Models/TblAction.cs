using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblAction", Schema = "dbo")]
    public partial class TblAction
    {
        [Key]
        [Column("ID_Action")]
        public int IdAction { get; set; }
        [Column("Action_ID")]
        public string ActionId { get; set; }
        public string LibAction { get; set; }
        public string DetailAction { get; set; }
        public string Statement { get; set; }
        public double Accurency { get; set; }
        public int Treshold { get; set; }
        public string Operator { get; set; }
        public string Complexaction { get; set; }
        //[ForeignKey("ID_Rule")]
        [Column("ID_Rule")]
        [UIHint("RuleActionId")]
        public int? IdRule { get; set; }

        public virtual TblRule IdRuleNavigation { get; set; }
    }
}
