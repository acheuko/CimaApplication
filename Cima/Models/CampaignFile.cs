using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cima.Models
{
    [Table("tblFileMask", Schema = "sysman")]
    public class CampaignFile
    {
        public CampaignFile()
        {
           Campaigns = new HashSet<Campaign>();
        }

        [Column("ID_FileMask")]
        public int CampaignFileId { get; set; }

        [Column("Libelle")]
        [Required]
        public string CampaignFileName { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}