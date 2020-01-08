
using System.ComponentModel.DataAnnotations;

namespace RestApi.Core.Domain.DTOs
{
	public class LoginCredentialDto
	{
		[Required]
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }

		[Required]
		public string GrantType { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }

		public string RefreshToken { get; set; }
	}
}
