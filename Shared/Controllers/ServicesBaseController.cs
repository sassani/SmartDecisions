using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Shared.Attributes;
using Shared.DTOs;

namespace Shared.Controllers
{
    public class ServicesBaseController : ControllerBase
    {
        protected string ServiceCode { get; set; }
        protected string ControllerCode { get; set; }
        protected Dictionary<string, string> Controllers { get; set; }

        protected string ErrorCode
        {
            get
            {
                MethodBase methodBase = new StackTrace().Get­Frame(1).GetMethod();
                MethodInfo methodInfo = methodBase.DeclaringType.GetRuntimeMethod(methodBase.Name, new Type[] { });
                if (methodInfo == null)// is an Async Method?
                {
                    Regex regex = new Regex(@"<(\w+)>.*");
                    string methodName = regex.Match(methodBase.DeclaringType.Name).Groups[1].Value;
                    methodInfo = methodBase.DeclaringType.ReflectedType.GetMethod(methodName);
                }
                var attrs = methodInfo.GetCustomAttributes(typeof(EndPointDataAttribute), true);
                if (attrs.Length != 0)
                {
                    EndPointDataAttribute attr = (EndPointDataAttribute)attrs[0];
                    return $"{ServiceCode}{GetControllerCode()}{attr.Code}";
                }
                return $"{ServiceCode}{GetControllerCode()}XX";
            }
        }

        protected virtual int GetLogsheetId()
        {
            return GetValidatedAccessToken().LogsheetId;
        }

        protected virtual string GetCredentialId()
        {
            return GetValidatedAccessToken().CredentialId;
        }



        private string ControllerName()
        {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        private string GetControllerCode()
        {
            return Controllers[ControllerName()];
        }

        private AccessToken GetValidatedAccessToken()
        {
            string HEADER_NAME = "XXX_VALIDATED_TOKEN";
            if (!Request.Headers.TryGetValue(HEADER_NAME, out StringValues token)) throw new Exception($"Header ({HEADER_NAME}) is required");
            return AccessToken.Decode(token);
        }
    }
}