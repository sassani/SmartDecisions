using OAuthService.Core.Domain;
using OAuthService.Core.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace OAuthService.DataBase.Persistence.Repositories
{
    public class CredentialRepo : Repo<Credential>, ICredentialRepo
    {
        private readonly new ApiContext context;
        public CredentialRepo(ApiContext context) : base(context)
        {
            this.context = context;
        }

        public bool IsEmailExist(string email)
        {
            var t = context.Credential
            .Where(u => u.Email.ToLower().Equals(email.ToLower()))
            .SingleOrDefault();
            if (t == null) return false;
            return true;
        }

        public Credential FindByEmail(string email)
        {
            return context.Credential
                .Where(cr => cr.Email.ToLower() == email.ToLower()).FirstOrDefault();
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
