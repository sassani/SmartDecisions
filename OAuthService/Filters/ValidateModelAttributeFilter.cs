using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuthService.Controllers.Responses;
using System.Collections.Generic;

namespace OAuthService.Filters
{
    public class ValidateModelAttributeFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<Error> errors = new List<Error>();
                foreach (var err in context.ModelState.Keys)
                {
                string errorMessage = "";
                    foreach (var subErr in context.ModelState[err].Errors)
                    {
                        errorMessage += subErr.ErrorMessage + " ";
                    }
                    errors.Add(new Error
                    {
                        Code = "000000",
                        Title = err,
                        Detail = errorMessage
                    });
                }
                context.Result = new Response(System.Net.HttpStatusCode.BadRequest, errors).ToActionResult();
            }
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // TODO: use this block to manage response layout and data
            //var result = context.Result;
            //// Do something with Result.
            //if (context.Canceled == true)
            //{
            //    // Action execution was short-circuited by another filter.
            //}

            //if (context.Exception != null)
            //{
            //    // Exception thrown by action or action filter.
            //    // Set to null to handle the exception.
            //    context.Exception = null;
            //}
            //base.OnActionExecuted(context);
        }
    }
}
