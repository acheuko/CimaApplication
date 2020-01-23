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
        public String Year { get; set; }

        [Required]
        public String Periode { get; set; }

        [Required]
        public String LibPeriodeCourt { get; set; }

        [Required]
        public String LibPeriodeLong { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }

        
        [Required]
        public DateTime EndDate { get; set; }

        
        public string Status { get; set; }

        
        public DateTime CreationDate { get; set; }

        public string Nom { get; set; }

        public string Code { get; set; }

        public string LibelleCampagne { get; set; }

        public virtual ICollection<CampaignFile> CampaignFiles { get; set; }

        public virtual ICollection<CampaignControl> CampaignControls { get; set; }
    }
}