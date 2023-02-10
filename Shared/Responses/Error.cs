using Newtonsoft.Json;

namespace Shared.Response
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
