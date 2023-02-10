using ApplicationService.Core.DAL.IRepositories;
using ApplicationService.Core.Domain;
using Shared.DAL;

namespace ApplicationService.DataBase.Repositories
{
    public class AvatarRepo:Repository<Avatar>, IAvatarRepo
    {
        public AvatarRepo(ApiContext context) : base(context)
        {

        }
    }
}
