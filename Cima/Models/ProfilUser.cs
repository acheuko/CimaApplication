using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cima.Models
{
    [Table("tblProfilUser", Schema = "sysman")]
    public class ProfilUser
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        [Column("ID_UserProfil")]
        public int ProfilUserId { get; set; }

        [ScaffoldColumn(false)]
        public string WebFolder { get; set; }

        [DisplayName("Libelle")]
        public string ProfilName { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string Description { get; set; }
    }
}