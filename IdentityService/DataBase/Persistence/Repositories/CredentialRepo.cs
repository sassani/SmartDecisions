using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityService.Core.DAL.IRepositories;
using IdentityService.Core.Domain;
using Shared.DAL;



namespace IdentityService.DataBase.Persistence.Repositories
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

        public async Task<Credential?> FindByEmailAsync(string email)
        {
            return await SingleOrDefaultAsync(cr => cr.Email.ToLower() == email.ToLower());
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
            credential.LastLoginAt = DateTime.UtcNow;
            context.Credential.Update(credential);
        }

        public async Task<Credential?> FindByUidAsync(string uid)
        {
            return await context.Credential.Where(cr => cr.PublicId == uid).FirstOrDefaultAsync();
        }
    }
}
