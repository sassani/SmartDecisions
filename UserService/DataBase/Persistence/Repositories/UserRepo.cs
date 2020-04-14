using Microsoft.EntityFrameworkCore;
using Shared.DAL;
using UserService.Core.Domain;

namespace UserService.DataBase.Persistence.Repositories
{
    public class UserRepo : Repository<User>
    {
        public UserRepo(DbContext context) : base(context)
        {
        }
    }
}
