using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblContratInterface", Schema = "sysman")]
    public class ContratInterface
    {
       
        [Column("ID_ContratInterface")]
        public int ContratInterfaceId { get; set; }

        [Required]
        public string Colonne { get; set; }
        
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Taille { get; set; }
        
        public string ValeursPossibles { get; set; }

        [Required]
        public string FileMask { get; set; }

    }
}