using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    public class Menu
    {
        [Key]
        public int ID_Auto { get; set; }

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Icon { get; set; }

        public string ParamUrl { get; set; }

        [ForeignKey("ID_Auto")]
        public Menu ParentID { get; set; }

        public List<Menu> MenuItems { get; set; }

    }
}