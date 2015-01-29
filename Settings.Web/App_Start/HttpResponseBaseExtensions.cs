using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Settings.Web
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string username, bool rememberMe, T userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(username, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, userData.ToString(), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            cookie.Value = encTicket;
            responseBase.Cookies.Add(cookie);

            return encTicket.Length;
        }
    }
}