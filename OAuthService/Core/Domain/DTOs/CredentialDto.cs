
using System.ComponentModel.DataAnnotations;

namespace OAuthService.Core.Domain.DTOs
{
	public class CredentialDto
	{
		public string GrantType { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string RefreshToken { get; set; }
		public string ResetPasswordToken { get; set; }
	}
}
