using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    [Table("tblProfilPrivilege", Schema = "sysman")]
    public class ProfilPrivilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_ProfilPrivilege")]
        public int ProfilPrivilegeId { get; set; }

        [Column("ID_MenuItems")]
        public string MenuItemId { get; set; }

        [Column("ID_Profil")]
        public string ProfilUserId { get; set; }
    }
}