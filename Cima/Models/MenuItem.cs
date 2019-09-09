using System;
using System.Collections.Generic;
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

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Icon { get; set; }


        public string ParamUrl { get; set; }

        [Column("ID_Parent")]
        [UIHint("GridForeignKey")]
        public int? MenuParentId { get; set; }

        [NotMapped]
        public List<MenuItem> MenuItems { get; set; }
    }
}