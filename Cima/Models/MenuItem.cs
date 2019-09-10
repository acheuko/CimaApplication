using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cima.Models
{
    [Table("tblMenuItems", Schema = "sysman")]
    public class MenuItem
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        [Column("ID_Auto")]
        public int MenuId { get; set; }

        [Column("ID_Parent")]
        [UIHint("GridForeignKey")]
        [DisplayName("Menu Parent")]
        public int? MenuParentId { get; set; }

        [DisplayName("Libelle")]
        public string Name { get; set; }

        [DisplayName("Controlleur")]
        public string Controller { get; set; }

        [DisplayName("Vue")]
        public string Action { get; set; }

        [DisplayName("Icône")]
        public string Icon { get; set; }

        [DisplayName("Paramètre")]
        public string ParamUrl { get; set; }

        [NotMapped]
        public List<MenuItem> MenuItems { get; set; }
    }
}