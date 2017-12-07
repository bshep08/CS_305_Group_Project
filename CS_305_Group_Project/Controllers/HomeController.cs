using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_305_Group_Project.Models;
namespace CS_305_Group_Project.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity.Name.Length == 0)
                return View();
            else
            {
                ApplicationUser user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                return View(user);
            }
        }
    }
}