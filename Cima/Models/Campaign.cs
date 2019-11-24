using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblCampaign", Schema = "sysman")]
    public class Campaign
    {
        public Campaign()
        {
            this.CampaignFiles = new HashSet<CampaignFile>();
            this.CampaignControls = new HashSet<CampaignControl>();
        }

        [Column("ID_Campaign")]
        public int CampaignId { get; set; }

       
        [Required]
        public DateTime BeginDate { get; set; }

        
        [Required]
        public DateTime EndDate { get; set; }

        
        public string Status { get; set; }

        
        public DateTime CreationDate { get; set; }

        public virtual ICollection<CampaignFile> CampaignFiles { get; set; }

        public virtual ICollection<CampaignControl> CampaignControls { get; set; }
    }
}