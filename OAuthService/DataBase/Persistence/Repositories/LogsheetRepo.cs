using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityService.Core.DAL.IRepositories;
using IdentityService.Core.Domain;
using Shared.DAL;

namespace IdentityService.DataBase.Persistence.Repositories
{
    public class LogsheetRepo : Repository<Logsheet>, ILogsheetRepo
    {
        private readonly new ApiContext context;

        public LogsheetRepo(ApiContext context) : base(context)
        {
            this.context = context;
        }

        public Task<Logsheet> FindLogsheetByRefreshTokenAsync(string refreshToken)
        {
            return context.Logsheet
                .Where(ls => ls.RefreshToken == refreshToken)
                .Include(ls => ls.Credential)
                .SingleOrDefaultAsync();
        }

        public void UpdateLastTimeLogin(Logsheet logSheet)
        {
            logSheet.UpdatedAt = DateTime.Now;
            context.Logsheet.Update(logSheet);
        }
    }
}
