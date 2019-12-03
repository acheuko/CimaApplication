using Cima.AppContext;
using Cima.Models;
using Cima.Repository.Shared;

namespace Cima.Repository
{
    public class REPO_Icon : SqlBaseRepository<Icon>
    {
        public REPO_Icon(SysmanDbContext context) : base(context)
        {

        }
    }
}