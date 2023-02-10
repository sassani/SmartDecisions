using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Shared.Response
{
    public class Response
    {
        [JsonPropertyName("data")]
        public object? Data { get; private set; }

        [JsonPropertyName("errors")]
        public IEnumerable<Error>? Errors { get; private set; }

        [JsonPropertyName("meta")]
        public Dictionary<string, object>? Meta { get; set; }

        [JsonPropertyName("links")]
        public object? Links { get; set; }

        private readonly HttpStatusCode status;

        public Response(HttpStatusCode status, IEnumerable<Error> errors)
        {
            this.status = status;
            Errors = errors;
        }
        public Response(HttpStatusCode status, Error error)
        {
            this.status = status;
            Errors = new List<Error>()
            {
                error
            };
        }

        public Response(HttpStatusCode status)
        {
            this.status = status;
            Data = new int[] { };
        }

        public Response(HttpStatusCode status, object data)
        {
            this.status = status;
            Data = data;
        }

        public Response(HttpStatusCode status, object data, object links)
        {
            this.status = status;
            Data = data;
            Links = links;
        }

        public Response(HttpStatusCode status, object data, Dictionary<string, object> meta)
        {
            this.status = status;
            Data = data;
            Meta = meta;
        }

        public Response(HttpStatusCode status, object data, object links, Dictionary<string, object> meta)
        {
            this.status = status;
            Data = data;
            Meta = meta;
            Links = links;
        }

        public override string ToString()
        {
            if (Data == null && Errors == null) Data = new int[] { };
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public ActionResult ToActionResult()
        {
            return new ContentResult()
            {
                ContentType = "application/json; charset=utf-8",
                Content = ToString(),
                StatusCode = (int)status
            };
        }

    }
}
