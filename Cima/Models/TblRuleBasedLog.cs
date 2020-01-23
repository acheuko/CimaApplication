using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Cima.Models
{
    [Table("tblRuleBasedLog", Schema = "dbo")]
    public partial class TblRuleBasedLog
    {
        public TblRuleBasedLog()
        {
            TblRuleBasedDetail = new HashSet<TblRuleBasedDetail>();
        }
        public TblRuleBasedLog(string seq, string detail, bool complete, DateTime datebegin, DateTime dateend)
        {
            Sequence = seq;
            Details = detail;
            Complete = complete;
            DateStart = datebegin;
            DateEnd = dateend;
        }

        public static void update(DbSet<TblRuleBasedLog> ltblrulebasedlog, TblRuleBasedLog tblrulebasedlog) 
        {
            if (tblrulebasedlog.IdRuleBasedLog > 0) { 
                //var _tblrulebasedlog = ltblrulebasedlog.
            }
        }

        [Key]
        [Column("ID_RuleBasedLog")]
        public int IdRuleBasedLog { get; set; }
        public string Sequence { get; set; }
        public string Details { get; set; }
        public bool Complete { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public virtual ICollection<TblRuleBasedDetail> TblRuleBasedDetail { get; set; }
    }
}
