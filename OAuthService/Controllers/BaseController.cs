using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OAuthService.Core.Domain;
using OAuthService.Core.Services.Interfaces;
using Shared;
using Shared.Controllers;

namespace OAuthService.Controllers
{
	public class BaseController : ServicesBaseController
    {
		protected readonly ICredentialService credentialSrvice;

		protected BaseController(ICredentialService credentialSrvice)
		{
			ServiceCode = GLOBAL_CONSTANTS.SERVICES.AUTH_SERVICE.CODE;
			Controllers = GLOBAL_CONSTANTS.SERVICES.AUTH_SERVICE.CONTROLLERS;
			this.credentialSrvice = credentialSrvice;
		}

		protected override int GetLogsheetId()
		{
			if (int.TryParse(User.FindFirst("lid")?.Value, out int id))
				return id;
			else
				return 0;
		}

		protected override string GetCredentialId()
		{
			var uid = User.FindFirst("uid")?.Value;
			return uid ?? "";
		}

		protected async Task<Credential> GetCredentialAsync()
		{
			return await credentialSrvice.CreateCredentialAsync(GetCredentialId());
		}
	}
}