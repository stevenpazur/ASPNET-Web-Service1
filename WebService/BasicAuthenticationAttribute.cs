using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebService
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                authenticationToken = authenticationToken.Replace(' ', '+');
                authenticationToken = authenticationToken.Replace('-', '+').Replace('_', '/');
                if(authenticationToken.Length % 4 > 0)
                {
                    authenticationToken += new string('=', 4 - authenticationToken.Length % 4);
                }
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                //string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                //string username = usernamePasswordArray[0];
                //string password = usernamePasswordArray[1];

                //if (EmployeeSecurity.Login(username, password))
                //{
                //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                //}
                //else
                //{
                //    actionContext.Response = actionContext.Request
                //    .CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                //}
            }
        }
    }
}