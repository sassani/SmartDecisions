using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OAuthService.Controllers.Responses
{
	public class Error
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Code { get; set; } = default!;

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Title { get; set; } = default!;

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? Detail { get; set; }
	}
}
