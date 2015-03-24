using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Macellum_Remake.Models;

namespace Macellum_Remake.Controllers
{
    /// <summary>
    /// Admin controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        readonly DatabaseRepo _repo = new DatabaseRepo();

        // GET: Admin
        public ActionResult Index()
        {
            var updated = _repo.GetLastDate();
            ViewBag.LastUpdate = updated;
            return View();
        }

        public ActionResult LoadData()
        {
            var dataclass = new DataClass();
            dataclass.dataLoad();

            return RedirectToAction("Index");
        }
    }
}