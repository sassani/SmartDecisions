using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuthService.Core.DAL.IRepositories;
using OAuthService.Core.Domain;
using Shared.DAL;



namespace OAuthService.DataBase.Persistence.Repositories
{
    public class CredentialRepo : Repository<Credential>, ICredentialRepo
    {
        private readonly new ApiContext context;
        public CredentialRepo(ApiContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            var t = await context.Credential
            .Where(u => u.Email.ToLower().Equals(email.ToLower()))
            .SingleOrDefaultAsync();
            if (t == null) return false;
            return true;
        }

        public async Task<Credential> FindByEmailAsync(string email)
        {
            return await context.Credential
                .Where(cr => cr.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
        }

        public CredentialRole[] GetRoles(Credential credential)
        {
            return context.CredentialRole
                .Where(ur => ur.CredentialId == credential.Id)
                .Include(r => r.Role)
                .ToArray();
        }

        public void UpdateLastLogin(Credential credential)
        {
            credential.LastLoginAt = DateTime.Now;
            context.Credential.Update(credential);
        }

        public async Task VerifyEmailAsync(string email)
        {
            Credential credential = context.Credential
                .Where(cr => cr.Email.ToLower() == email.ToLower()).FirstOrDefault();
            credential.IsEmailVerified = true;
            await Task.Run(() =>
            {
                context.Credential.Update(credential);
                context.SaveChanges();
            });
        }

        public async Task<Credential> FindByUidAsync(string uid)
        {
            return await context.Credential.Where(cr => cr.PublicId == uid).FirstOrDefaultAsync();
        }
    }
}
