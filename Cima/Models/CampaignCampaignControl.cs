using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    [Table("tblCampaignControl", Schema = "sysman")]
    public class CampaignCampaignControl
    {
        [Key]
        [Column("ID_CampaignControl")]
        public int CampaignControlId { get; set; }

        [Column("ID_Campaign")]
        public string CampaignId { get; set; }

        [Column("ID_ControlType")]
        public string ControlId { get; set; }
               
        public string Blocking { get; set; }

    }
}