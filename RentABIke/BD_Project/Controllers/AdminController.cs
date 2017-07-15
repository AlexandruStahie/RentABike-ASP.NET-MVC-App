using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class AdminController : Controller
    {
        private DBContext dbContext = new DBContext();

        [Authorize(Users = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Users = "admin")]
        public ActionResult AdminPage()
        {
            return View();
        }

    }
}