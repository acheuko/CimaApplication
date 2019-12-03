using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    [Table("tblDerogation", Schema = "sysman")]
    public class Derogation
    {
        [Column("ID_Derogation")]
        public int DerogationId { get; set; }

        [Required]
        public string Motif { get; set; }

        [Required]
        public string Campagne { get; set; }

        [Required]
        public string Fichier { get; set; }

        public string Raison { get; set; }

        [Required]
        public string Statut { get; set; }
       
    }
}