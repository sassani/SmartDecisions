using System.Collections.Generic;

namespace OAuthService.Core.Domain
{
	public class Role
	{
		public Role()
		{
            CredentialRole = new HashSet<CredentialRole>();
		}
		public int Id { get; set; }
		public AppEnums.RoleType Type { get; set; }
		//public string Name { get; set; }

		public virtual ICollection<CredentialRole> CredentialRole { get; set; }
	}
}
