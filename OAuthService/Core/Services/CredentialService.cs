using OAuthService.Core.Domain;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Core.DataServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services
{
	public class CredentialService : ICredentialService
	{
		private readonly IUnitOfWork unitOfWork;
		public CredentialService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public Credential Get(int userId)
		{
			Credential user = new Credential();
			//UserDb userDb = unitOfWork.User.Get(userId);
			//Mapper.UserMapper(user, userDb);
			return user;
		}

		public Credential Get(string uuid)
		{
			Credential user = new Credential();
			//UserDb userDb = unitOfWork.User.Get(userId);
			//Mapper.UserMapper(user, userDb);
			return user;
		}

		public bool CheckEmail(string email){
			return unitOfWork.Credential.IsEmailExist(email);
		}

		public void AddUserByUserInfo(RegisterUserDto user)
		{
			//UserDb newUser = new UserDb
			//{
			//	FirstName = user.FirstName,
			//	LastName = user.LastName,
			//	Email = user.Email,
			//	Password = StringHelper.StringToHash(user.Password)
			//};
			//unitOfWork.User.Add(newUser);
			//unitOfWork.Complete();
		}
	}
}
