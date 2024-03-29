﻿
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Core.Domain.DTOs
{
	public class CredentialDto
	{
		public string RequestType { get; set; } = default!;
		public string? ClientId { get; set; }
		public string? ClientSecret { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? NewPassword { get; set; }
		public string? RefreshToken { get; set; }
		public string? ResetPasswordToken { get; set; }
	}
}
