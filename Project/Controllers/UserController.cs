using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.EF;
using TaskManagementSystem.Auth;

namespace TaskManagementSystem.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        private TaskManagementEntities1 db = new TaskManagementEntities1();

        public static User Convert(UserDTO d)
        {
            return new User
            {
                UserId = d.UserId,
                Name = d.Name,
                Email = d.Email,
                Password = d.Password,
                Role = d.Role
            };
        }

        public static UserDTO Convert(User d)
        {
            return new UserDTO
            {
                UserId = d.UserId,
                Name = d.Name,
                Email = d.Email,
                Password = d.Password,
                Role = d.Role
            };
        }

        public static List<UserDTO> Convert(List<User> data)
        {
            var list = new List<UserDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }

        [Logged]
        public ActionResult List()
        {
            var data = db.Users.ToList();
            return View(Convert(data));
        }

        [Logged]
        [HttpGet]
        public ActionResult Create()
        {
            return View(new User());
        }

        [Logged]
        [HttpPost]
        public ActionResult Create(UserDTO d)
        {
            //
            if (ModelState.IsValid)
            {
                db.Users.Add(Convert(d));
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(d);
        }

        [Logged]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ID is null or not provided.");
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound("User not found.");
            }

            return View(user);
        }

        [Logged]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Logged]
        [HttpPost]
        public ActionResult Edit(UserDTO d)
        {
            var exobj = db.Users.Find(d.UserId);
            db.Entry(exobj).CurrentValues.SetValues(d);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [AdminAccess]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var exobj = db.Users.Find(id);
            return View(Convert(exobj));
        }

        [AdminAccess]
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var exobj = db.Users.Find(Id);
                db.Users.Remove(exobj);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}