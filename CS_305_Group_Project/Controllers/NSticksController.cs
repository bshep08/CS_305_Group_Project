using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_305_Group_Project.Models;

namespace CS_305_Group_Project.Controllers
{
    public class NSticksController : Controller
    {
        private ApplicationDbContext DbContext;

        public NSticksController() {
            DbContext = new ApplicationDbContext();
        }

        // GET: NSticks
        public ActionResult Index()
        {
            string UserName = User.Identity.Name;

            //FOR THE LOVE OF GOD TAKE THIS OUT AND REPLACE WITH ACTUAL IDENTIFICATION
            if (UserName.Equals("security@officer.org")) {
                return RedirectToAction("SecurityIndex", "NSticks");
            }

            if (UserName.Length == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<NStick> Sticks = new List<NStick>();

                foreach (NStick nStick in DbContext.NSticks)
                {
                    if (nStick.UserName != null && nStick.UserName.Equals(UserName))
                    {
                        Sticks.Add(nStick);
                    }
                }

                return View(Sticks);
            }
        }

        public ActionResult SecurityIndex() {
            string UserName = User.Identity.Name;

            //FOR THE LOVE OF GOD TAKE THIS OUT AND REPLACE WITH ACTUAL IDENTIFICATION
            if (UserName.Length != 0 && UserName.Equals("security@officer.org"))
            {
                var Sticks = DbContext.NSticks.ToList();
                return View(Sticks);
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult New()
        {
            string UserName = User.Identity.Name;

            if (UserName.Length == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            else
                return View();
        }

        public ActionResult Add(NStick nStick) {

            string UserName = User.Identity.Name;

            if (UserName.Length > 0)
            {
                try
                {
                    nStick.UserName = UserName;
                    DbContext.NSticks.Add(nStick);
                    DbContext.SaveChanges();
                }
                catch (Exception e) {
                    return RedirectToAction("Index");
                }
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id) {
            var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

            if (nStick == null)
            {
                return HttpNotFound();
            }

            return View(nStick);
        }

        public ActionResult Delete(int id) {
            var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

            if (nStick == null)
            {
                return HttpNotFound();
            }

            return View(nStick);
        }

        public ActionResult PerformDelete(int id) {
            var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

            if (nStick == null)
            {
                return HttpNotFound();
            }

            DbContext.NSticks.Remove(nStick);
            DbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(NStick nStick) {
            var Db_nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == nStick.Id);

            if (Db_nStick == null)
                return HttpNotFound();

            try
            {
                Db_nStick.TimeStuck = nStick.TimeStuck;
                Db_nStick.Description = nStick.Description;

                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}