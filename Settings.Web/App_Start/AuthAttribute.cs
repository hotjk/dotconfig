using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Settings.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthAttribute : FilterAttribute, IAuthorizationFilter 
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                FormsIdentity formIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                FormsAuthenticationTicket ticket = formIdentity.Ticket as FormsAuthenticationTicket;
                string userData = ticket.UserData;
                return;
            }
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            string url = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;
            filterContext.Result = new RedirectResult(urlHelper.Action("Login", "Home"));// + "?ReturnUrl=" + HttpUtility.UrlEncode(url));
            //FormsAuthentication.RedirectToLoginPage("returnurl=" + url);
        }
    }
}