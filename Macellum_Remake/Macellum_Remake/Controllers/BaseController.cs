using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Macellum_Remake.Models;

namespace Macellum_Remake.Controllers
{
    /// <summary>
    /// Base controller for auth filters
    /// </summary>
    public class BaseController : Controller
    {
        private readonly DatabaseRepo _repo = new DatabaseRepo();

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrEmpty(SimpleSessionPersister.Username))
            {
                var sessionId = _repo.GetSessionId(SimpleSessionPersister.Username);

                if (sessionId == System.Web.HttpContext.Current.Session.SessionID)
                    filterContext.HttpContext.User = new MyPrinciple(new MyIdentity(SimpleSessionPersister.Username));
                else
                {
                    SimpleSessionPersister.Username = null;
                }
            }

            base.OnAuthorization(filterContext);
        }
    }
}