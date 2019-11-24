using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;

namespace Cima.Repository
{
    public class REPO_CampaignCampaignControl : SqlBaseRepository<CampaignCampaignControl>
    {
        public REPO_CampaignCampaignControl(SysmanDbContext context) : base(context)
        {

        }
    }
}