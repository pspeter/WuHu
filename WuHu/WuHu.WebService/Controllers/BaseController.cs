using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using WuHu.WebService.Providers;

namespace WuHu.WebService.Controllers
{
    public class BaseController : ApiController
    {
        protected ApplicationRoleManager AppRoleManager => 
            Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
    }
}
