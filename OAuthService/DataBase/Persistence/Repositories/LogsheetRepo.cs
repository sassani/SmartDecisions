using OAuthService.Core.Domain;
using OAuthService.Core.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.DataBase.Persistence.Repositories
{
    public class LogsheetRepo : Repo<Logsheet>, ILogsheetRepo
    {
        private readonly new ApiContext context;

        public LogsheetRepo(ApiContext context) : base(context)
        {
            this.context = context;
        }

        public Task<Logsheet> FindLogsheetByRefreshTokenAsync(string refreshToken)
        {
            DateTime currentDay = DateTime.Now;

            return Task.Run(() =>
            {
                return context.Logsheet
                 .Where(ls => ls.RefreshToken == refreshToken)
                 .Include(cr => cr.Credential)
                 .SingleOrDefaultAsync();
            });
        }

        public void UpdateLastTimeLogin(Logsheet logSheet)
        {
            logSheet.UpdatedAt = DateTime.Now;
            context.Logsheet.Update(logSheet);
        }
    }
}
