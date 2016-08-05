﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.Filters
{
    public class AuthorizeMenu : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Controller.ViewBag.AutherizationMessage = "Custom Authorization: Message from OnAuthorization method.";
        }

        Entities context = new Entities(); // my entity  
            private readonly string[] allowedroles;
            public CustomAuthorizeAttribute(params string[] roles)
            {
                this.allowedroles = roles;
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                bool authorize = false;
                foreach (var role in allowedroles)
                {
                    var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
                    m.IsActive == true); // checking active users with allowed roles.  
                    if (user.Count() > 0)
                    {
                        authorize = true; /* return true if Entity has current user(active) with specific role */
                    }
                }
                return authorize;
            }
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }



    }
}
