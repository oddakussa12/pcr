using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class BasecampConnectionController : Controller
    {
        // GET: Connect
        public ActionResult Basecamp()
        {
            
            var server = new DotNetOpenAuth.OAuth2.AuthorizationServerDescription();
            server.AuthorizationEndpoint = new Uri("https://launchpad.37signals.com/authorization/new?type=web_server");
            

            server.TokenEndpoint = new Uri("https://launchpad.37signals.com/authorization/token?type=web_server");

            var client = new DotNetOpenAuth.OAuth2.WebServerClient(
                server, "0f8b523480cadae39c3941362721d775803a8ebf", "84431ef8b42b3ab561359586230140f6bc97e70e");

            //client.RequestUserAuthorization(returnTo: new Uri("http://localhost:3757/connect/basecampauth"));
            client.RequestUserAuthorization(returnTo: new Uri("https://localhost:44369/"));
            
            //Response.End();

            return null;
        }

        [HttpPost]
        public ActionResult BasecampAuth()
        {
            var server = new DotNetOpenAuth.OAuth2.AuthorizationServerDescription();
            server.AuthorizationEndpoint = new Uri("https://launchpad.37signals.com/authorization/new?type=web_server");
            server.TokenEndpoint = new Uri("https://launchpad.37signals.com/authorization/token");


            var client = new DotNetOpenAuth.OAuth2.WebServerClient(
                server, "0f8b523480cadae39c3941362721d775803a8ebf", "84431ef8b42b3ab561359586230140f6bc97e70e");

            //var state = client.ProcessUserAuthorization(Request);
            //Response.Write(state.AccessToken);
            //Response.End();
            return null;
        }

    }
}
