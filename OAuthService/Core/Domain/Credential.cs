using System;
using System.Collections.Generic;

namespace OAuthService.Core.Domain
{
	public class Credential
	{
        public Credential()
        {
            CredentialRole = new HashSet<CredentialRole>();
            Logsheet = new HashSet<Logsheet>();
        }
        public int Id { get; set; }
		public string PublicId { get; set; }
		public string Email { get; set; }
        public string Password { get; set; }
		public bool IsEmailVerified { get; set; }
		public bool IsActive { get; set; }
		public DateTime? LastLoginAt { get; set; }

        //ignored properties:
        public bool IsAuthenticated { get; set; }

        public virtual ICollection<CredentialRole> CredentialRole { get; set; }
        public virtual ICollection<Logsheet> Logsheet { get; set; }

    }

}
