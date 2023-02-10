using RestApi.DataBase.Core.Domain;


namespace RestApi.DataBase.Persistence.Repositories.Interfaces
{
	public interface IUser : IRepo<UserDb>
	{
		bool IsEmailExist(string email);
		UserDb FindByEmail(string email);
		UserRole[] GetRoles(UserDb user);
		void UpdateLastLogin(int userId);
	}

}
