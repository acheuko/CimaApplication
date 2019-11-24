using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cima.Models
{
    [Table("tblIcons", Schema = "sysman")]
    public class Icon
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int IconId { get; set; }

        [Column("icon")]
        public string IconValue { get; set; }

        [Column("libelle")]
        public string Libelle { get; set; }

    }
}