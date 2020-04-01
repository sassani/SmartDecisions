using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.Core.Domain
{
	public class Logsheet
	{
		public int Id { get; set; }
		public string RefreshToken { get; set; } = default!;

		public int CredentialId { get; set; }
		public virtual Credential Credential { get; set; } = new Credential();

		public int ClientId { get; set; }
		public virtual Client Client { get; set; } = new Client();

		public string? Platform { get; set; }
		public string? Browser { get; set; }
		public string? IP { get; set; }


		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
