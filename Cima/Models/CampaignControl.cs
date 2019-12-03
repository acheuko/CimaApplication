using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblControl", Schema = "sysman")]
    public class CampaignControl
    {
        public CampaignControl()
        {
            this.Campaigns = new HashSet<Campaign>();
        }

        public int CampaignControlId { get; set; }
        [Required]
        public string CampaignControlName { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}