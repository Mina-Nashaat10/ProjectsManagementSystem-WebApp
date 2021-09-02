using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{

    public class HomeController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();
        // GET: Home
        [HttpGet]
        public ActionResult Index(int? x)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Comment c)
        {

            return View();
        }
        public ActionResult fareed()
        {
            return View();
        }
        [HttpGet]
        public ActionResult login()
        {
            List<Role> actors = new List<Role>();
            actors = db.Roles.ToList();
            ViewBag.Actors = actors;
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection form)
        {
            string type = form["ActorType"];
            string email = form["username"];
            string password = form["password"];
            if (ModelState.IsValid)
            {
                using (PMSDBEntities p = new PMSDBEntities())
                {
                    if (type == "Admin")
                    {
                        var c = p.Admins.Where(x => x.Ad_Email.Equals(email) && x.Ad_Password.Equals(password)).SingleOrDefault();
                        if (c != null)
                        {
                            var r = p.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                            Session["roleid"] = r.rid.ToString();
                            Session["actorid"] = c.Ad_id.ToString();
                            Session["actorname"] = c.Ad_FirstName + " " + c.Ad_LastName;
                            Session["actortype"] = "Admin";
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            List<Role> actor = new List<Role>();
                            actor = p.Roles.ToList();
                            ViewBag.Actors = actor;
                            return View();
                        }
                    }
                    else if (type == "Customer")
                    {
                        var c = p.Customers.Where(x => x.Cust_Email.Equals(email) && x.Cust_Password.Equals(password)).SingleOrDefault();
                        if (c != null)
                        {
                            var r = p.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                            Session["roleid"] = r.rid.ToString();
                            Session["actorid"] = c.Cust_ID.ToString();
                            Session["actorname"] = c.Cust_FirstName + " " + c.Cust_LastName;
                            Session["actortype"] = "Customer";
                            return RedirectToAction("Index", "Customer");
                        }
                        else
                        {
                            List<Role> actor = new List<Role>();
                            actor = p.Roles.ToList();
                            ViewBag.Actors = actor;
                            return View();
                        }
                    }
                    else if (type == "PM")
                    {

                        var a = db.ProjectManagers.Where(x => x.PM_Email.Equals(email) && x.PM_Password.Equals(password)).SingleOrDefault();
                        if (a != null)
                        {
                            var r = p.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                            Session["roleid"] = r.rid.ToString();
                            Session["actorid"] = a.PM_id.ToString();
                            Session["actorname"] = a.PM_FirstName + " " + a.PM_LastName;
                            Session["actortype"] = "Project Manager";
                            return RedirectToAction("PreviousProjectsandcurrent", "PM");
                        }
                        else
                        {
                            List<Role> actor = new List<Role>();
                            actor = db.Roles.ToList();
                            ViewBag.Actors = actor;
                            return View();
                        }
                    }
                    else if (type == "TL")
                    {
                        var a = db.TeamLeaders.Where(x => x.TL_Email.Equals(email) && x.TL_Password.Equals(password)).SingleOrDefault();
                        if (a != null)
                        {
                            var r = p.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                            Session["roleid"] = r.rid.ToString();
                            Session["actorid"] = a.TL_ID.ToString();
                            Session["actorname"] = a.TL_FirstName + " " + a.TL_LastName;
                            Session["actortype"] = "Team Leader";
                            return RedirectToAction("PreviousProjectsandcurrent", "TL");
                        }
                        else
                        {
                            List<Role> actor = new List<Role>();
                            actor = db.Roles.ToList();
                            ViewBag.Actors = actor;
                            return View();
                        }
                    }
                    else
                    {
                        var a = db.JuniorDevelopers.Where(x => x.JD_Email.Equals(email) && x.JD_Password.Equals(password)).SingleOrDefault();
                        if (a != null)
                        {
                            var r = p.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                            Session["roleid"] = r.rid.ToString();
                            Session["actorid"] = a.JD_ID.ToString();
                            Session["actorname"] = a.JD_FirstName + " " + a.JD_LastName;
                            Session["actortype"] = "Junior Developers";
                            return RedirectToAction("Index", "JD");
                        }
                        else
                        {
                            List<Role> actor = new List<Role>();
                            actor = db.Roles.ToList();
                            ViewBag.Actors = actor;
                            return View();
                        }
                    }
                }
            }
            else
            {
                List<Role> actor = new List<Role>();
                actor = db.Roles.ToList();
                ViewBag.Actors = actor;
                return View();
            }


        }
        [HttpGet]
        public ActionResult register()
        {
            List<Role> actor = new List<Role>();
            actor = db.Roles.ToList();
            ViewBag.Actors = actor;
            return View();
        }
        [HttpPost]
        public ActionResult register(FormCollection form)
        {
            string type = form["ActorType"];

            if (ModelState.IsValid)
            {
                if (type == "Admin")
                {
                    Admin a = new Admin();
                    a.Ad_FirstName = form["firstname"];
                    a.Ad_LastName = form["lastname"];
                    a.Ad_Email = form["email"];
                    a.Ad_Password = form["password"];
                    a.Ad_Mobile = form["mobile"];
                    a.Ad_JobDescription = form["job"];
                    a.Ad_image = form["photo"];
                    db.Admins.Add(a);
                    db.SaveChanges();
                    var r = db.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                    Session["roleid"] = r.rid.ToString();
                    Session["actorid"] = a.Ad_id.ToString();
                    Session["actorname"] = a.Ad_FirstName + " " + a.Ad_LastName;
                    Session["actortype"] = "Admin";
                    return RedirectToAction("Index", "Admin");
                }
                else if (type == "Customer")
                {
                    Customer c = new Customer();
                    c.Cust_FirstName = form["firstname"];
                    c.Cust_LastName = form["lastname"];
                    c.Cust_Email = form["email"];
                    c.Cust_Password = form["password"];
                    c.Cust_Mobile = form["mobile"];
                    c.Cust_JobDescription = form["job"];
                    c.Cust_image = form["photo"];
                    db.Customers.Add(c);
                    db.SaveChanges();
                    var r = db.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                    Session["roleid"] = r.rid.ToString();
                    Session["actorid"] = c.Cust_ID.ToString();
                    Session["actorname"] = c.Cust_FirstName + " " + c.Cust_LastName;
                    Session["actortype"] = "Customer";
                    int k = int.Parse(Session["actorid"].ToString());
                    var s = db.Notifications.Where(w => w.Actor2_name.Equals("Customer") && w.Person2_Id == k).ToList();
                    ViewBag.allnotcust = s;
                    return RedirectToAction("Index", "Customer");
                }
                else if (type == "TL")
                {
                    TeamLeader a = new TeamLeader();
                    a.TL_FirstName = form["firstname"];
                    a.TL_LastName = form["lastname"];
                    a.TL_Email = form["email"];
                    a.TL_Password = form["password"];
                    a.TL_Mobile = form["mobile"];
                    a.TL_JobDescription = form["job"];
                    a.TL_image = form["photo"];
                    db.TeamLeaders.Add(a);
                    db.SaveChanges();
                    var r = db.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                    Session["roleid"] = r.rid.ToString();
                    Session["actorid"] = a.TL_ID.ToString();
                    Session["actorname"] = a.TL_FirstName + " " + a.TL_LastName;
                    Session["actortype"] = "Team Leader";
                    int k = int.Parse(Session["actorid"].ToString());
                    var s = db.Notifications.Where(w => w.Actor2_name.Equals("TL") && w.Person2_Id == k).ToList();
                    ViewBag.allnottls = s;
                    return RedirectToAction("PreviousProjectsandcurrent", "TL");
                }
                else if (type == "PM")
                {
                    ProjectManager a = new ProjectManager();
                    a.PM_FirstName = form["firstname"];
                    a.PM_LastName = form["lastname"];
                    a.PM_Email = form["email"];
                    a.PM_Password = form["password"];
                    a.PM_Mobile = form["mobile"];
                    a.PM_Jobdescription = form["job"];
                    a.PM_image = form["photo"];
                    db.ProjectManagers.Add(a);
                    db.SaveChanges();
                    var r = db.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                    Session["roleid"] = r.rid.ToString();
                    Session["actorid"] = a.PM_id.ToString();
                    Session["actorname"] = a.PM_FirstName + " " + a.PM_LastName;
                    Session["actortype"] = "Project Manager";
                    int k = int.Parse(Session["actorid"].ToString());
                    var s = db.Notifications.Where(w => w.Actor2_name.Equals("PM") && w.Person2_Id == k).ToList();
                    ViewBag.allnotpm = s;
                    return RedirectToAction("PreviousProjectsandcurrent", "PM");
                }
                else
                {
                    JuniorDeveloper a = new JuniorDeveloper();
                    a.JD_FirstName = form["firstname"];
                    a.JD_LastName = form["lastname"];
                    a.JD_Email = form["email"];
                    a.JD_Password = form["password"];
                    a.JD_Mobile = form["mobile"];
                    a.JD_JobDescription = form["job"];
                    a.JD_image = form["photo"];
                    db.JuniorDevelopers.Add(a);
                    db.SaveChanges();
                    var r = db.Roles.Where(x => x.rname.Equals(type)).SingleOrDefault();
                    Session["roleid"] = r.rid.ToString();
                    Session["actorid"] = a.JD_ID.ToString();
                    Session["actorname"] = a.JD_FirstName + " " + a.JD_LastName;
                    Session["actortype"] = "Junior Developers";
                    int k = int.Parse(Session["actorid"].ToString());
                    var s = db.Notifications.Where(w => w.Actor2_name.Equals("JD") && w.Person2_Id == k).ToList();
                    ViewBag.allnotjds = s;
                    return RedirectToAction("Index", "JD");
                }
            }
            else
            {
                List<Role> actor = new List<Role>();
                actor = db.Roles.ToList();
                ViewBag.Actors = actor;
                return View();
            }
        }


    }
}