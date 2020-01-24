using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace OAuthService.Controllers
{
	/// <summary>
	/// </summary>
	/// <error-code>01</error-code>
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AuthController : BaseController
	{
		private readonly ICredentialService credentialService;
		private readonly IClientService clientService;

		public AuthController(ICredentialService credentialSrvice, IClientService clientService, ICredentialService credentialService) : base(credentialSrvice)
		{
			ErrorCode = "01";
			this.credentialService = credentialService;
			this.clientService = clientService;
		}
		
		/// <summary>
		/// </summary>
		/// <error-code>01</error-code>
		/// <param name="login User Credential"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost()]
		public async Task<IActionResult> Login([FromBody] CredentialDto loginCredential)
		{
			string errCode = "01";
			Client client = await clientService.CreateClientAsync(loginCredential.ClientId,loginCredential.ClientSecret);

			if (!client.IsValid)
			{
				return new Response(HttpStatusCode.Forbidden,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"01",
							Title = "Invalid Client",
							Detail = "Client info is incorrect."
						} }).ToActionResult();
			}

			Credential credential = await credentialService.CreateCredential(loginCredential);
			if (credential.IsAuthenticated)
			{
				// check user
				if (!credential.IsActive)
				{
					return new Response(HttpStatusCode.Forbidden,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"04",
							Detail = "Your account is suspended"
						} }).ToActionResult();
				}

				//if (!credential.IsEmailVerified)
				//{
				//	return new Response(HttpStatusCode.Forbidden,
				//		new Error[] { new Error {
				//			Code = ErrorCode+errCode+"05",
				//			Detail = "Your email is not verified"
				//		} }).ToActionResult();
				//}

				var payload = new
				{
					authToken = credentialService.Login(client, credential)
				};
				return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
			}
			else
			{
				if (loginCredential.GrantType.ToLower().Equals("refreshtoken"))
				{
					return new Response(HttpStatusCode.Forbidden,
					new Error[] { new Error {
						Code = ErrorCode+errCode+"02",
						Detail = "Refresh token is incorrect or expired."
					} }).ToActionResult();
				}
				return new Response(HttpStatusCode.Forbidden,
					new Error[] { new Error {
						Code = ErrorCode+errCode+"03",
						Detail = "Email or password is incorrect."
					} }).ToActionResult();
			}

		}

		[HttpDelete()]
		public IActionResult Logout()
		{
			string errCode = "02";
			if (credentialService.Logout(GetLogsheetId()))
			{

				return new Response(HttpStatusCode.Accepted, null).ToActionResult();
			}

			return new Response(HttpStatusCode.BadGateway,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"01",
							Title = "Invalid data.",
							Detail = "Maybe you have signed out before!"
						} }).ToActionResult();
		}

		[HttpDelete("all")]
		public IActionResult LogoutAll()
		{
			string errCode = "03";
			if (credentialService.Logout(GetLogsheetId(), true))
			{
				return new Response(HttpStatusCode.Accepted, null).ToActionResult();
			}

			return new Response(HttpStatusCode.BadGateway,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"01",
							Title = "Invalid data.",
							Detail = "Maybe you have signed out before!"
						} }).ToActionResult();
		}
	}
}