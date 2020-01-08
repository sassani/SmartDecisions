using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;

namespace RestApi.Core.Services.Interfaces
{
	public interface ICredentialService
	{
		Credential Get(int userId);
		Credential Get(string uuid);
		bool CheckEmail(string email);
		void AddUserByUserInfo(RegisterUserDto user);

	}
}
