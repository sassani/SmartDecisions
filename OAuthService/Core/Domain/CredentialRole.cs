using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.Core.Domain
{
	public class CredentialRole
	{
		public int Id { get; set; }

		public int CredentialId { get; set; }
		public virtual Credential Credential { get; set; }

		public int RoleId { get; set; }
		public virtual Role Role { get; set; }
	}
}
