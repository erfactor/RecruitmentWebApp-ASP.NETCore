using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Recruitment.Core;

namespace Recruitment.Utils
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UserTypeAttribute : ActionMethodSelectorAttribute
    {
        private readonly UserType _userType;

        public UserTypeAttribute(UserType userType)
        {
            _userType = userType;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (!routeContext.HttpContext.User.Identity.IsAuthenticated)
                return false;
            var nameIdentifier = routeContext.HttpContext.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value;
            var emailidentifier = routeContext.HttpContext.User.FindFirst("emails").Value;
            
            var userType = UserTypeManager.StaffData.CheckUserType(nameIdentifier);
            if (userType != UserType.Admin)
            {
                userType = UserTypeManager.StaffData.CheckUserType(emailidentifier);
            }
            return _userType == userType;
        }
    }
}