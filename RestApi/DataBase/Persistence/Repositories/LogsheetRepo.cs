using RestApi.Core.Domain;
using RestApi.Core.DataServices.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RestApi.DataBase.Persistence.Repositories
{
    public class LogsheetRepo : Repo<Logsheet>, ILogsheetRepo
    {
        private readonly new ApiContext context;

        public LogsheetRepo(ApiContext context) : base(context)
        {
            this.context = context;
        }

        public Logsheet FindByRefreshToken(string refreshToken)
        {
            DateTime currentDay = DateTime.Now;

            return context.Logsheet
                .Where(uc => uc.RefreshToken == refreshToken && currentDay - uc.CreatedAt <= TimeSpan.FromDays(73))
                .Include(u => u.Credential)
                //.ThenInclude(uc => uc.UserRole)
                .SingleOrDefault();
        }
    }
}
