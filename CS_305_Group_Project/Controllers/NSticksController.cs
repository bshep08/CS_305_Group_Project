using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_305_Group_Project.Models;
using System.Net.Mail;

namespace CS_305_Group_Project.Controllers
{
    public class NSticksController : Controller
    {
        private ApplicationDbContext DbContext;

        public NSticksController()
        {
            DbContext = new ApplicationDbContext();
        }

        // GET: NSticks
        public ActionResult Index()
        {
            if (User.Identity.Name.Length == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (user.Role.Equals("SecurityOfficer"))
                {
                    return RedirectToAction("SecurityIndex", "NSticks");
                }
                else if (user.Role.Equals("Admin"))
                {
                    return RedirectToAction("AdminIndex", "NSticks");
                }
                else
                {
                    List<NStick> Sticks = new List<NStick>();

                    foreach (NStick nStick in DbContext.NSticks)
                    {
                        if (nStick.UserName != null && nStick.UserName.Equals(User.Identity.Name))
                        {
                            Sticks.Add(nStick);
                        }
                    }

                    return View(Sticks);
                }
            }
        }

        public ActionResult New()
        {
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("User"))
                {
                    return View();
                }
                else
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult Add(NStick nStick)
        {
            if (nStick == null)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("User"))
                {
                    try
                    {
                        nStick.UserName = User.Identity.Name;
                        nStick.ComparableDate = Convert.ToDateTime(nStick.TimeStuck);
                        DbContext.NSticks.Add(nStick);
                        DbContext.SaveChanges();

                        if (user.Verified)
                        {
                            var safety = from s in DbContext.Users where s.Role == "SecurityOfficer" select s;
                            List<ApplicationUser> users = safety.ToList();

                            MailMessage mail = new MailMessage();
                            foreach (ApplicationUser u in users)
                            {
                                mail.To.Add(u.Email);
                            }

                            // mail.To.Add("Another Email ID where you wanna send same email");
                            mail.From = new MailAddress("wahjudizhangsong@gmail.com");
                            mail.Subject = "New Needle Stick Report";

                            string Body = user.UserName + " has just filed a new needle stick report";
                            mail.Body = Body;

                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Port = 587;
                            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                            smtp.Credentials = new System.Net.NetworkCredential
                            ("wahjudizhangsong@gmail.com", "NeedleStick1!");
                            //Or your Smtp Email ID and Password
                            smtp.EnableSsl = true;
                            try
                            {
                                smtp.Send(mail);
                            }
                            catch (Exception e)
                            {
                                return RedirectToAction("Index");
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                    return RedirectToAction("Index");

                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(int id = -1)
        {
            if (id <= -1) {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("User"))
                {
                    var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

                    if (nStick == null)
                    {
                        return HttpNotFound();
                    }

                    return View(nStick);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Delete(int id = -1)
        {
            if (id <= -1)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("User"))
                {
                    var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

                    if (nStick == null)
                    {
                        return HttpNotFound();
                    }

                    return View(nStick);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult PerformDelete(int id = -1)
        {
            if (id <= -1)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                var nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

                if (nStick == null)
                {
                    return HttpNotFound();
                }

                DbContext.NSticks.Remove(nStick);
                DbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Update(NStick nStick)
        {
            if (nStick == null)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                var Db_nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == nStick.Id);

                if (Db_nStick == null)
                    return HttpNotFound();

                try
                {
                    Db_nStick.TimeStuck = nStick.TimeStuck;
                    Db_nStick.Description = nStick.Description;
                    Db_nStick.Location = nStick.Location;

                    DbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //BEGIN SAFETY OFFICER CODE

        public ActionResult SecurityIndex(string sortOrder, string searchString, DateTime? beginDate, DateTime? endDate)
        {
            if (String.IsNullOrEmpty(sortOrder))
            {
                ViewBag.HandleSortParm = "Handle";
            }
            else
                ViewBag.HandleSortParm = sortOrder == "Handle" ? "handle_desc" : "Handle";

            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var Sticks = from s in DbContext.NSticks select s;
                    var Users = DbContext.Users.ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Sticks = Sticks.Where(s => s.UserName.Contains(searchString));
                    }

                    switch (sortOrder)
                    {
                        case "Handle":
                            Sticks = Sticks.OrderBy(n => n.Handled);
                            break;
                        default:
                            Sticks = Sticks.OrderByDescending(n => n.Handled);
                            break;
                    }

                    if (beginDate != null && endDate != null && beginDate >= endDate) {
                        ViewBag.BeginDate = endDate;
                        ViewBag.EndDate = beginDate;
                        Sticks = Sticks.Where(s => s.ComparableDate >= endDate && s.ComparableDate <= beginDate);
                    }
                    else if (beginDate != null && endDate != null) {
                        ViewBag.BeginDate = beginDate;
                        ViewBag.EndDate = endDate;
                        Sticks = Sticks.Where(s => s.ComparableDate >= beginDate && s.ComparableDate <= endDate);
                    }
                    else if (beginDate != null) {
                        ViewBag.BeginDate = beginDate;
                        Sticks = Sticks.Where(s => s.ComparableDate >= beginDate);
                    }
                    else if (endDate != null)
                    {
                        ViewBag.EndDate = endDate;
                        Sticks = Sticks.Where(s => s.ComparableDate <= endDate);
                    }

                    List<NStick> sticks = new List<NStick>();

                    var officer = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

                    foreach (NStick nStick in Sticks)
                    {
                        foreach (ApplicationUser User in Users)
                        {
                            if (nStick.UserName == User.UserName && User.WorkPlaceName == officer.WorkPlaceName && User.Verified)
                            {
                                sticks.Add(nStick);
                            }
                        }
                    }

                    return View(sticks);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult SetHandled(int id = -1)
        {
            if (id <= -1)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var Db_nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

                    if (Db_nStick == null)
                        return HttpNotFound();

                    return View(Db_nStick);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult PerformHandled(int id = -1)
        {
            if (id <= -1)
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var Db_nStick = DbContext.NSticks.SingleOrDefault(n => n.Id == id);

                    if (Db_nStick == null)
                        return HttpNotFound();

                    try
                    {
                        Db_nStick.Handled = !Db_nStick.Handled;

                        DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult SecurityUsers(string searchString, string nameString, string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder))
            {
                ViewBag.HandleSortParm = "Verified";
            }
            else
                ViewBag.HandleSortParm = sortOrder == "Verified" ? "verified_desc" : "Verified";
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var Users = from s in DbContext.Users select s;
                    List<ApplicationUser> users = new List<ApplicationUser>();
                    var officer = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Users = Users.Where(s => s.UserName.Contains(searchString));
                    }
                    if (!String.IsNullOrEmpty(nameString)) {
                        Users = Users.Where(s => s.FirstName.Contains(nameString) || s.LastName.Contains(nameString));
                    }

                    switch (sortOrder)
                    {
                        case "Verified":
                            Users = Users.OrderBy(u => u.Verified);
                            break;
                        default:
                            Users = Users.OrderByDescending(u => u.Verified);
                            break;
                    }


                    foreach (ApplicationUser User in Users)
                    {
                        if (User.WorkPlaceName == officer.WorkPlaceName && User.Email != officer.Email && !User.Role.Equals("Admin"))
                        {
                            users.Add(User);
                        }
                    }

                    

                    return View(users);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult SetVerified(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }

                    return View(User);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult PerformVerification(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "NSticks");
            }
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("SecurityOfficer"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }

                    try
                    {
                        User.Verified = !User.Verified;
                        DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("SecurityUsers");
                    }

                    return RedirectToAction("SecurityUsers");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        //BEGIN ADMIN CODE

        public ActionResult AdminIndex()
        {
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var Sticks = DbContext.NSticks.ToList();
                    var Users = DbContext.Users.ToList();

                    List<NStick> sticks = new List<NStick>();

                    var admin = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

                    foreach (NStick nStick in Sticks)
                    {
                        foreach (ApplicationUser User in Users)
                        {
                            if (nStick.UserName == User.UserName && User.WorkPlaceName == admin.WorkPlaceName)
                            {
                                sticks.Add(nStick);
                            }
                        }
                    }

                    return View(sticks);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult AdminUsers(string searchString, string nameString)
        {
            if (User.Identity.Name.Length > 0)
            {
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var Users = from s in DbContext.Users select s;
                    List<ApplicationUser> users = new List<ApplicationUser>();
                    var admin = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Users = Users.Where(s => s.UserName.Contains(searchString));
                    }
                    if (!String.IsNullOrEmpty(nameString))
                    {
                        Users = Users.Where(s => s.FirstName.Contains(nameString) || s.LastName.Contains(nameString));
                    }

                    foreach (ApplicationUser User in Users)
                    {
                        if (User.WorkPlaceName == admin.WorkPlaceName && User.Email != admin.Email)
                        {
                            users.Add(User);
                        }
                    }

                    return View(users);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult AdminChangeUserRole(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("AdminUsers");
            }
            if (User.Identity.Name.Length > 0)
            {
                
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }

                    return View(User);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult PerformChangeRole(ApplicationUser appUser) {
            if (appUser == null)
            {
                return RedirectToAction("AdminUsers");
            }

            if (User.Identity.Name.Length > 0)
            {
                
                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == appUser.Id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }

                    try
                    {
                        User.Role = appUser.Role;
                        DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("AdminUsers");
                    }

                    return RedirectToAction("AdminUsers");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                 return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult DeleteUser(string id) {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("AdminUsers");
            }
            if (User.Identity.Name.Length > 0)
            {

                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }

                    return View(User);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult PerformDeleteUser(string id) {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("AdminUsers");
            }
            if (User.Identity.Name.Length > 0)
            {

                ApplicationUser user;
                try
                {
                    user = DbContext.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (user.Role.Equals("Admin"))
                {
                    var User = DbContext.Users.SingleOrDefault(u => u.Id == id);
                    if (User == null)
                    {
                        return HttpNotFound();
                    }
                    try
                    {
                        DbContext.Users.Remove(User);
                        DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("AdminUsers");
                    }
                    return RedirectToAction("AdminUsers");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}