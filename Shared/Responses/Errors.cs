using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorHandlers;
using Shared.Response;

namespace Shared.Responses
{
    public static class Errors
    {
        public static IActionResult InternalServer(string code = null, string title = null, string detail = null)
        {
            var err = new Error
            {
                Code = code != null ? code : "500",
                Title = title != null ? title : "Internal Error",
                Detail = detail != null ? detail : "Please try again later"
            };
            return new Response.Response(HttpStatusCode.InternalServerError, err).ToActionResult();
        }

        public static IActionResult Forbiden(string code = null, string title = null, string detail = null)
        {
            var err = new Error
            {
                Code = code != null ? code : "403",
                Title = title != null ? title : "Access Forbidden",
                Detail = detail != null ? detail : "Access to this resource is Forbidden for this user Id"
            };
            return new Response.Response(HttpStatusCode.Forbidden, err).ToActionResult();
        }

        public static IActionResult NotFound(string code = null, string title = null, string detail = null)
        {
            var err = new Error
            {
                Code = code != null ? code : "404",
                Title = title != null ? title : "Empty Resource",
                Detail = detail != null ? detail : "There is no resource here"
            };
            return new Response.Response(HttpStatusCode.NotFound, err).ToActionResult();
        }

        public static IActionResult Conflict(string code = null, string title = null, string detail = null)
        {
            var err = new Error
            {
                Code = code != null ? code : "409",
                Title = title != null ? title : "Resource Conflict",
                Detail = detail != null ? detail : "There is already a same resource registered"
            };
            return new Response.Response(HttpStatusCode.Conflict, err).ToActionResult();
        }

        public static IActionResult BadRequest(string code = null, string title = null, string detail = null)
        {
            var err = new Error
            {
                Code = code != null ? code : "400",
                Title = title != null ? title : "Bad Request",
                Detail = detail != null ? detail : "yuoe request is not valid"
            };
            return new Response.Response(HttpStatusCode.BadRequest, err).ToActionResult();
        }

        public static IActionResult BaseExceptionResponse(BaseException exception, string code = null)
        {
            var err = new Error
            {
                Code = code != null ? code : exception.Status.ToString(),
                Title = exception.Title != null ? exception.Title : "Internal Server Error",
                Detail = exception.Description != null ? exception.Description : "There is something wrong in our system. Sorry about that."
            };
            return new Response.Response(exception.Status, err).ToActionResult();
        }
    }
}
