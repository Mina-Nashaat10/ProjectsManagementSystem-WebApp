using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{
    public class JDController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();
        // GET: JD
        public ActionResult Index()
        {
            int cc = int.Parse(Session["actorid"].ToString());

            //Current Project
            var a = db.JdCurrentProjects.Where(x => x.Jd_id == cc).ToList();
            List<CurrentProject> curr = new List<CurrentProject>();
            foreach (var item in a)
            {
                CurrentProject cp = new CurrentProject();
                cp = db.CurrentProjects.Where(x => x.Post_ID == item.Post_id).First();
                curr.Add(cp);
            }
            List<ProjectManager> pms = new List<ProjectManager>();
            List<TeamLeader> tls = new List<TeamLeader>();
            List<Customer> custs = new List<Customer>();
            foreach (var item in curr)
            {
                var aa = db.ProjectManagers.Where(x => x.PM_id == item.PM_ID).First();
                ProjectManager p = new ProjectManager();
                p.PM_FirstName = aa.PM_FirstName;
                p.PM_LastName = aa.PM_LastName;
                pms.Add(p);

                var b = db.TeamLeaders.Where(x => x.TL_ID == item.TL_ID).First();
                TeamLeader t = new TeamLeader();
                t.TL_FirstName = b.TL_FirstName;
                t.TL_LastName = b.TL_LastName;
                tls.Add(t);

                var c = db.Customers.Where(x => x.Cust_ID == item.Cust_ID).First();
                Customer cr = new Customer();
                cr.Cust_FirstName = c.Cust_FirstName;
                cr.Cust_LastName = c.Cust_LastName;
                custs.Add(cr);

            }
            ViewBag.allcurr = curr;
            ViewBag.allpms = pms;
            ViewBag.alltls = tls;
            ViewBag.allcusts = custs;


            //Previous Project
            var w = db.JdPreProjects.Where(x => x.JD_id == cc).ToList();
            List<PreProject> pre = new List<PreProject>();
            foreach (var item in w)
            {
                PreProject pp = new PreProject();
                pp = db.PreProjects.Where(x => x.Post_ID == item.Post_id).First();
                pre.Add(pp);
            }
            List<ProjectManager> pms1 = new List<ProjectManager>();
            List<TeamLeader> tls1 = new List<TeamLeader>();
            List<Customer> custs1 = new List<Customer>();
            foreach (var item in pre)
            {
                var aa = db.ProjectManagers.Where(x => x.PM_id == item.PM_ID).First();
                ProjectManager p = new ProjectManager();
                p.PM_FirstName = aa.PM_FirstName;
                p.PM_LastName = aa.PM_LastName;
                pms1.Add(p);

                var b = db.TeamLeaders.Where(x => x.TL_ID == item.TL_ID).First();
                TeamLeader t = new TeamLeader();
                t.TL_FirstName = b.TL_FirstName;
                t.TL_LastName = b.TL_LastName;
                tls1.Add(t);

                var c = db.Customers.Where(x => x.Cust_ID == item.Cust_ID).First();
                Customer cr = new Customer();
                cr.Cust_FirstName = c.Cust_FirstName;
                cr.Cust_LastName = c.Cust_LastName;
                custs1.Add(cr);

            }
            ViewBag.allpre = pre;
            ViewBag.allpms1 = pms1;
            ViewBag.alltls1 = tls1;
            ViewBag.allcusts1 = custs1;


            var gg = db.JuniorDevelopers.Where(f => f.JD_ID == cc).SingleOrDefault();
            ViewBag.inf = gg;


            int kk = int.Parse(Session["actorid"].ToString());
            var ss = db.Notifications.Where(t => t.Actor2_name.Equals("JD") && t.Person2_Id == kk).ToList();
            ViewBag.allnotjds = ss;

            // get my current notification
            int jdId = int.Parse(Session["actorid"].ToString());
            List<Notification> myNotification = new List<Notification>();
            myNotification = db.Notifications.Where(i => i.Person2_Id == jdId && i.Actor2_name == "JD").ToList();
            ViewBag.allNotificationForJD = myNotification;

            //get project mangers names that send these notifications
            List<String> projectManagersNames = new List<string>();
            ProjectManager pm;

            // get project content for these notifications
            List<Project> notificationProjects = new List<Project>();
            for (int i = 0; i < myNotification.Count; i++)
            {
                // get team leader name for this notification
                pm = new ProjectManager();
                int projectManagerId = (int)myNotification[i].Person1_Id;
                pm = db.ProjectManagers.Where(s => s.PM_id == projectManagerId).Single();
                projectManagersNames.Add(pm.PM_FirstName + " " + pm.PM_LastName);
                int PostId = (int)myNotification[i].Post_ID;
                notificationProjects.Add(db.Projects.Where(s => s.Post_ID == PostId).FirstOrDefault());
            }
            ViewBag.projectManagersNames = projectManagersNames;
            ViewBag.posts = notificationProjects;

            return View();
        }
        public ActionResult rejectPmRequest(int pmId, int postId)
        {
            if (ModelState.IsValid)
            {
                int jdId = int.Parse(Session["actorid"].ToString());
                String actorJdName = "JD";

                String actorPmName = "PM";
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == pmId && i.Actor1_Name == actorPmName && i.Person2_Id == jdId && i.Actor2_name == actorJdName && i.Post_ID == postId).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();

                Notification newNotification = new Notification();
                newNotification.Person1_Id = jdId;
                newNotification.Actor1_Name = actorJdName;

                newNotification.Person2_Id = pmId;
                newNotification.Actor2_name = actorPmName;

                newNotification.Message = "Sorry, I'm very busy...";
                newNotification.Date = DateTime.Now.ToString();
                newNotification.Post_ID = (int)postId;
                db.Notifications.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("Index", "JD");
            }
            return View();
        }
        public ActionResult acceptPmRequest(int pmId, int postId)
        {
            if (ModelState.IsValid)
            {
                int jdId = int.Parse(Session["actorid"].ToString());
                // delete old Notification
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == pmId && i.Actor1_Name == "PM" && i.Person2_Id == jdId && i.Actor2_name == "JD" && i.Post_ID == postId).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();

                // add project to jd current projects
                JdCurrentProject jdCurrentProject = new JdCurrentProject();
                jdCurrentProject.Jd_id = jdId;
                jdCurrentProject.Post_id = postId;
                db.JdCurrentProjects.Add(jdCurrentProject);
                db.SaveChanges();

                Notification newNotification = new Notification();
                newNotification.Person1_Id = jdId;
                newNotification.Actor1_Name = "JD";

                newNotification.Person2_Id = pmId;
                newNotification.Actor2_name = "PM";
                newNotification.Message = "I Agree...";
                newNotification.Date = DateTime.Now.ToString();
                newNotification.Post_ID = postId;
                db.Notifications.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("Index", "JD");

            }
            return View();
        }
    }
}