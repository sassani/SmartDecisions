using RestApi.Core.Domain;
using RestApi.Core.DataServices.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace RestApi.DataBase.Persistence.Repositories
{
	public class CredentialRepo : Repo<Credential>, ICredentialRepo
	{
		private readonly new ApiContext context;
		public CredentialRepo(ApiContext context) : base(context)
		{
			this.context = context;
		}
		public bool IsEmailExist(string email){
			//var t = context.User
			//.Where(u => u.Email.ToLower().Equals(email.ToLower()))
			//.SingleOrDefault();
			//if(t!=null)return false;
			return true;
		}

		public Credential FindByEmail(string email)
		{
            return context.Credential.SingleOrDefault();
				//.Where(u => u.Email.ToLower() == email.ToLower())
				//.Include(u => u.UserRole)
				//.ThenInclude(r => r.Role)
				//.SingleOrDefault();
		}

		public CredentialRole[] GetRoles(Credential user)
		{
			return context.CredentialRole
				.Where(ur => ur.CredentialId == user.Id)
				.Include(r => r.Role)
				.ToArray();
		}

		public void UpdateLastLogin(int userId)
		{
			var user = context.Credential.SingleOrDefault(u => u.Id == userId);
			user.LastLoginAt = DateTime.Now;
			context.Credential.Update(user);
		}



	}
}
