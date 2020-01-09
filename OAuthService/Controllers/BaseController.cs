
using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain;
using OAuthService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OAuthService.Controllers
{
	public class BaseController : ControllerBase
    {
		protected string ErrorCode { get; set; }

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
			return User.FindFirst("uid")?.Value;
		}

		//protected int GetUserId()
		//{
		//	if (int.TryParse(User.FindFirst("uid")?.Value, out int id))
		//		return id;
		//	else
		//		return 0;
		//}

		protected Credential GetCredential()
		{
			return credentialSrvice.Get(GetCredentialId());
		}

		protected IActionResult MakeResponse(System.Net.HttpStatusCode statusCode, object payload = null)
		{
			return new Response(statusCode, payload).ToActionResult();
		}

		protected IActionResult MakeErrorResponse(System.Net.HttpStatusCode statusCode, Error error)
		{
			return new Response(statusCode, new Error[] { error }).ToActionResult();
		}
	}
}