using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    [Table("tblCampaignFileMask", Schema = "sysman")]
    public class CampaignCampaignFile
    {
        [Key]
        [Column("ID_CampaignFileMask")]
        public int CampaignFileId { get; set; }

        [Column("ID_Campaign")]
        public string CampaignId { get; set; }
        
        [Column("ID_FileMask")]
        public string FileId { get; set; }


    }
}