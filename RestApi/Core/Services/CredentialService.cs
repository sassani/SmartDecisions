using RestApi.Core.Domain;
using RestApi.Helpers;
using RestApi.Core.Services.Interfaces;
using RestApi.Core.DataServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.Core.Domain.DTOs;

namespace RestApi.Core.Services
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
