
using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain;
using OAuthService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OAuthService.Controllers
{
	public class BaseController : ControllerBase
    {
		protected string ErrorCode { get; set; } = "00";

		protected readonly ICredentialService credentialSrvice;

		protected BaseController(ICredentialService credentialSrvice)
		{
			this.credentialSrvice = credentialSrvice;
		}

		protected int GetLogsheetId()
		{
			if (int.TryParse(User.FindFirst("lid")?.Value, out int id))
				return id;
			else
				return 0;
		}

		protected string GetCredentialId()
		{
			var uid = User.FindFirst("uid")?.Value;
			return uid ?? "";
		}

		protected async Task<Credential> GetCredentialAsync()
		{
			return await credentialSrvice.CreateCredentialAsync(GetCredentialId());
		}

		//protected IActionResult MakeResponse(System.Net.HttpStatusCode statusCode, object? payload = null)
		//{
		//	return new Response(statusCode, payload).ToActionResult();
		//}

		//protected IActionResult MakeErrorResponse(System.Net.HttpStatusCode statusCode, Error error)
		//{
		//	return new Response(statusCode, new Error[] { error }).ToActionResult();
		//}
	}
}