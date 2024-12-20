using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TaskManagementSystem.EF;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Auth;

namespace TaskManagementSystem.Controllers
{
    public class TaskController : Controller
    {
        // GET: Tas
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

        public static Task Convert(TaskDTO d)
        {
            return new Task
            {
                TaskName = d.TaskName,
                TaskId = d.TaskId,
                Description = d.Description,
                Deadline = d.Deadline,
                UserId = d.UserId
            };
        }

        public static TaskDTO Convert(Task d)
        {
            return new TaskDTO
            {
                TaskName = d.TaskName,
                TaskId = d.TaskId,
                Description = d.Description,
                Deadline = d.Deadline,
                UserId = d.UserId
            };
        }

        public static List<TaskDTO> Convert(List<Task> data)
        {
            var list = new List<TaskDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }

        [Logged]
        public ActionResult List()
        {
            var data = db.Tasks.ToList();

            return View(Convert(data));
        }

        [AdminAccess]
        [HttpGet]
        public ActionResult Create(int UserId)
        {
            var author = db.Users.Find(UserId);
            if (author == null)
            {
                return HttpNotFound("Author not found.");
            }

            var taskDTO = new TaskDTO
            {
                UserId = UserId
            };

            var users = db.Users.ToList();

            return View(taskDTO);
        }

        [AdminAccess]
        [HttpPost]
        public ActionResult Create(TaskDTO d)
        {
            if (ModelState.IsValid)
            {
                var taskEntity = Convert(d);

                if (!db.Users.Any(a => a.UserId == d.UserId))
                {
                    ModelState.AddModelError("", "Invalid User ID.");
                    return View(d);
                }

                db.Tasks.Add(taskEntity);
                db.SaveChanges();

                return RedirectToAction("List", "Task");
            }

            return View(d);
        }

        [Logged]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [Logged]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [Logged]
        [HttpPost]
        public ActionResult Edit(TaskDTO d)
        {
            var exobj = db.Tasks.Find(d.TaskId);
            db.Entry(exobj).CurrentValues.SetValues(d);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [AdminAccess]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var exobj = db.Tasks.Find(id);
            return View(Convert(exobj));
        }

        [AdminAccess]
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var exobj = db.Tasks.Find(Id);
                db.Tasks.Remove(exobj);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}