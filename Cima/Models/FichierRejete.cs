using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    [Table("tblFichierRejete", Schema = "sysman")]
    public class FichierRejete
    {
        [Column("ID_FichierRejete")]
        public int FichierRejeteId { get; set; }

        public string Filename { get; set; }

        public string Reason { get; set; }
    }
}