using TaskManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Logini
        private TaskManagementEntities1 db = new TaskManagementEntities1();

        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public ActionResult Index(LoginDTO log)
        {
            if (ModelState.IsValid)
            {
                var user = (from u in db.Users
                            where u.Email.Equals(log.Email)
                            && u.Password.Equals(log.Password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user;
                    if (user.Role == 2) { return RedirectToAction("List", "User"); }
                    return RedirectToAction("List", "User");
                }
            }
            return View(log);
        }
    }
}