using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;

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
            return int.Parse(User.FindFirst("lid")?.Value);
        }

        protected virtual string GetCredentialId()
        {
            return User.FindFirst("uid")?.Value;
        }

        private string ControllerName()
        {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        private string GetControllerCode()
        {
            return Controllers[ControllerName()];
        }
    }
}