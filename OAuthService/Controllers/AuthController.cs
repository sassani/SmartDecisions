using RestApi.Controllers.Responses;
using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;
using RestApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestApi.Controllers
{
	/// <summary>
	/// </summary>
	/// <error-code>01</error-code>
	[Authorize]
	[ApiController]
	[Route("api/auth")]
	public class AuthController : BaseController
	{
		private readonly IAuthService authService;
		private readonly IClientService clientService;

		public AuthController(ICredentialService credentialSrvice, IClientService clientService, IAuthService authService) : base(credentialSrvice)
		{
			ErrorCode = "01";
			this.authService = authService;
			this.clientService = clientService;
		}
		
		/// <summary>
		/// </summary>
		/// <error-code>01</error-code>
		/// <param name="login User Credential"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost()]
		public IActionResult Login([FromBody] LoginCredentialDto loginCredential)
		{
			string errCode = "01";
			Client client = clientService.CreateClient(loginCredential.ClientId,loginCredential.ClientSecret);

			if (!client.IsValid)
			{
				return new Response(HttpStatusCode.Forbidden,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"01",
							Title = "Invalid Client",
							Detail = "Client info is incorrect."
						} }).ToActionResult();
			}

			Credential credential = new Credential();
			if (authService.Authenticate(loginCredential, ref credential))
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

				if (!credential.IsEmailVerified)
				{
					return new Response(HttpStatusCode.Forbidden,
						new Error[] { new Error {
							Code = ErrorCode+errCode+"05",
							Detail = "Your email is not verified"
						} }).ToActionResult();
				}

				var payload = new
				{
					authToken = authService.Login(client, credential)
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
			if (authService.Logout(GetLogsheetId()))
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
			if (authService.Logout(GetLogsheetId(), true))
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