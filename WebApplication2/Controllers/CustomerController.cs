using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CustomerController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();
        // GET: Customer
        public ActionResult Index()
        {
            int cc = int.Parse(Session["actorid"].ToString());
            var allpost = db.CurrentProjects.Where(x => x.Cust_ID == cc).ToList();
            List<TeamLeader> tls = new List<TeamLeader>();
            List<ProjectManager> pms = new List<ProjectManager>();
            foreach (var item in allpost)
            {
                if(item.TL_ID != null)
                {
                    var d = db.TeamLeaders.Where(x => x.TL_ID == item.TL_ID).First();
                    TeamLeader l = new TeamLeader();
                    l.TL_FirstName = d.TL_FirstName;
                    l.TL_LastName = d.TL_LastName;
                    tls.Add(l);
                }
               else
                {
                    TeamLeader tl = new TeamLeader();
                    tl.TL_FirstName = "NOT";
                    tl.TL_LastName = "Assign";
                    tls.Add(tl);
                }

                var e = db.ProjectManagers.Where(x => x.PM_id == item.PM_ID).First();
                ProjectManager p = new ProjectManager();
                p.PM_FirstName = e.PM_FirstName;
                p.PM_LastName = e.PM_LastName;
                pms.Add(p);
            }
            ViewBag.tlnames = tls;
            ViewBag.pmnames = pms;
            ViewBag.currentproj = allpost;

            //Previous Projects
            var preprojects = db.PreProjects.Where(x => x.Cust_ID == cc).ToList();
            List<TeamLeader> tls1 = new List<TeamLeader>();
            List<ProjectManager> pms1 = new List<ProjectManager>();
            foreach (var item in preprojects)
            {
                var d = db.TeamLeaders.Where(x => x.TL_ID == item.TL_ID).First();
                TeamLeader l = new TeamLeader();
                l.TL_FirstName = d.TL_FirstName;
                l.TL_LastName = d.TL_LastName;
                tls1.Add(l);

                var e = db.ProjectManagers.Where(x => x.PM_id == item.PM_ID).First();
                ProjectManager p = new ProjectManager();
                p.PM_FirstName = e.PM_FirstName;
                p.PM_LastName = e.PM_LastName;
                pms1.Add(p);
            }
            ViewBag.tlnames1 = tls1;
            ViewBag.pmnames1 = pms1;
            ViewBag.previousprojects = preprojects;
            var gg = db.Customers.Where(f => f.Cust_ID == cc).SingleOrDefault();
            ViewBag.inf = gg;


            int k = int.Parse(Session["actorid"].ToString());
            var s = db.Notifications.Where(w => w.Actor2_name.Equals("Customer") && w.Person2_Id == k).ToList();
            ViewBag.allnotcust = s;

            string[] names = new string[s.Count()];
            int ii = 0;
            foreach (var item in s)
            {
                int bb = s[ii].Person1_Id;
                var f = db.ProjectManagers.Where(z => z.PM_id == bb).First();
                names[ii] = f.PM_FirstName + " " + f.PM_LastName;
                ii++;
            }
            ViewBag.allpmss = names;
            string[] names11 = new string[s.Count()];
            ii = 0;
            foreach (var item in s)
            {
                int bb = s[ii].Post_ID;
                var f = db.Projects.Where(z => z.Post_ID == bb).First();
                names11[ii] = f.Post_Description;
                ii++;
            }
            ViewBag.allposts = names11;
            return View();
        }
        [HttpGet]
        public ActionResult searchpm()
        {
            var s = db.ProjectManagers.ToList();
            ViewBag.allpms = s;
            return View();
        }
        public static int? a;
        [HttpGet]
        public ActionResult sendNotifications(int? id)
        {
            int w = int.Parse(Session["actorid"].ToString());
            var s = db.Projects.Where(h => h.Cust_ID == w).ToList();
            ViewBag.allprojects = s;
            if(id!=null)
            {
                a = id;
            }
            return View();
        }
        [HttpPost]
        public ActionResult sendNotifications(FormCollection form)
        {
            if(ModelState.IsValid)
            {
                Notification n = new Notification();
                n.Person1_Id = int.Parse(Session["actorid"].ToString());
                int q = int.Parse(Session["roleid"].ToString());
                var x = db.Roles.Where(b => b.rid == q).SingleOrDefault();
                n.Actor1_Name = x.rname;
                n.Person2_Id = a;
                n.Actor2_name = "PM";
                n.Message = form["message"];
                n.Date = DateTime.Now.ToString();
                string g = form["postdes"];
               var m = db.Projects.Where(o => o.Post_Description.Equals(g)).First();
                n.Post_ID = m.Post_ID;
                db.Notifications.Add(n);
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                int w = int.Parse(Session["actorid"].ToString());
                var s = db.Projects.Where(h => h.Cust_ID == w).ToList();
                ViewBag.allprojects = s;
            }
            return View();
        }
        public ActionResult deletenot(int id , int actor)
        {
            if(ModelState.IsValid)
            {
                var s = db.Notifications.Where(k => k.Actor1_Name.Equals("PM") && k.Actor2_name.Equals("Customer") && k.Person2_Id == id && k.Post_ID == actor).First(); ;
                Notification n = new Notification();
                n = s;
                db.Notifications.Remove(n);
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            return View();
        }
    }
}