
namespace WebApplication2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using WebApplication2.Models;
    public class AdminController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();
        // GET: Admin
        public static string error = "null";

        public ActionResult Index()
        {
            var customers = db.Customers.ToList();
            ViewBag.customers = customers;

            var pms = db.ProjectManagers.ToList();
            ViewBag.pm = pms;

            var tls = db.TeamLeaders.ToList();
            ViewBag.tl = tls;

            var jds = db.JuniorDevelopers.ToList();
            ViewBag.jd = jds;

            var posts = db.Projects.ToList();
            ViewBag.post = posts;

            var currentpro = db.CurrentProjects.ToList();


            List<TeamLeader> teamleader1 = new List<TeamLeader>();
            List<Customer> cust1 = new List<Customer>();
            List<ProjectManager> curpm = new List<ProjectManager>();
            for (int j = 0; j < currentpro.Count(); j++)
            {
                int s = (int)currentpro[j].TL_ID;
                var t = db.TeamLeaders.Where(v => v.TL_ID == s).First();
                TeamLeader tl = new TeamLeader();
                tl.TL_FirstName = t.TL_FirstName;
                tl.TL_LastName = t.TL_LastName;
                teamleader1.Add(tl);

                int k = (int)currentpro[j].Cust_ID;
                var d = db.Customers.Where(b => b.Cust_ID == k).First();
                Customer r = new Customer();
                r.Cust_FirstName = d.Cust_FirstName;
                r.Cust_LastName = d.Cust_LastName;
                cust1.Add(r);

                int e = (int)currentpro[j].PM_ID;
                var q = db.ProjectManagers.Where(x => x.PM_id == e).First();
                ProjectManager p = new ProjectManager();
                p.PM_FirstName = q.PM_FirstName;
                p.PM_LastName = q.PM_LastName;
                curpm.Add(p);
            }

            ViewBag.team1 = teamleader1;
            ViewBag.customers1 = cust1;
            ViewBag.allpm = curpm;
            ViewBag.allprojects1 = currentpro;

            int[] cust_id = new int[posts.Count()];
            int i = 0;
            foreach (var item in posts)
            {
                cust_id[i] = item.Cust_ID;
                i++;
            }
            i = 0;
            string[] cust_name = new string[posts.Count()];
            foreach (var item in posts)
            {
                var s = db.Customers.Where(c => c.Cust_ID == item.Cust_ID).SingleOrDefault();
                cust_name[i] = s.Cust_FirstName + " " + s.Cust_LastName;
                i++;
            }
            ViewBag.custnames = cust_name;
             
            int id = int.Parse(Session["actorid"].ToString());
            var admin = db.Admins.Where(x => x.Ad_id == id).SingleOrDefault();
            ViewBag.inf = admin;

            if (error != "null")
            {
                ViewData["aa"] = error;
            }
            else
            {
                ViewData["aa"] = "null";
            }
            return View();
        }
        public ActionResult remove(int id,string actor)
        {
            if(ModelState.IsValid)
            {
                if (actor.Equals("PM"))
                {
                    List<CurrentProject> a = new List<CurrentProject>();
                    a = db.CurrentProjects.Where(x => x.TL_ID == id).ToList();
                    List<PreProject> b = new List<PreProject>();
                    b = db.PreProjects.Where(y => y.TL_ID == id).ToList();
                    List<Comment> c = new List<Comment>();
                    c = db.Comments.Where(z => z.PM_ID == id).ToList();
                    if (a.Count.Equals(0) && b.Count.Equals(0) && c.Count.Equals(0))
                    {
                        ProjectManager p = db.ProjectManagers.Find(id);
                        db.ProjectManagers.Remove(p);
                        db.SaveChanges();
                        error = "null";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error = "Can Not Remove The Project Manager";
                        return RedirectToAction("Index");
                    }
                }
                else if(actor.Equals("TL"))
                {
                    List<CurrentProject> a = new List<CurrentProject>();
                    a = db.CurrentProjects.Where(x => x.TL_ID == id).ToList();
                    List<PreProject> b = new List<PreProject>();
                    b = db.PreProjects.Where(y => y.TL_ID == id).ToList();
                    if (a.Count.Equals(0) && b.Count.Equals(0))
                    {
                        TeamLeader t = t = db.TeamLeaders.Find(id);
                        db.TeamLeaders.Remove(t);
                        db.SaveChanges();
                        error = "null";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error = "Can Not Remove The Team Leader";
                        return RedirectToAction("Index");
                    }
                }
                else if (actor.Equals("JD"))
                {
                    List<JdCurrentProject> a = new List<JdCurrentProject>();
                    a = db.JdCurrentProjects.Where(x => x.Jd_id == id).ToList();
                    List<JdCurrentProject> b = new List<JdCurrentProject>();
                    b = db.JdCurrentProjects.Where(y => y.Jd_id == id).ToList();
                    if (a.Count.Equals(0) && b.Count.Equals(0))
                    {
                        JuniorDeveloper j = j = db.JuniorDevelopers.Find(id);
                        db.JuniorDevelopers.Remove(j);
                        db.SaveChanges();
                        error = "null";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error = "Can Not Remove The Junior Developers";
                        return RedirectToAction("Index");
                    }
                    
                }
                else
                {
                    List<CurrentProject> a = new List<CurrentProject>();
                    a = db.CurrentProjects.Where(x => x.Cust_ID == id).ToList();
                    List<PreProject> b = new List<PreProject>();
                    b = db.PreProjects.Where(y => y.Cust_ID == id).ToList();
                    List<Project> c = new List<Project>();
                    c = db.Projects.Where(y => y.Cust_ID == id).ToList();
                    if (a.Count.Equals(0) && b.Count.Equals(0) && c.Count.Equals(0))
                    {
                        Customer cr = db.Customers.Find(id);
                        db.Customers.Remove(cr);
                        db.SaveChanges();
                        error = "null";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error = "Can Not Remove The Customer";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();

        }
        [HttpGet]
        public ActionResult add_admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_admin(FormCollection form)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index"); 

            }

            return View();
        }
        [HttpGet]
        public ActionResult add_pm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_pm(FormCollection form)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index");

            }

            return View();
        }
        [HttpGet]
        public ActionResult add_tl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_tl(FormCollection form)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index");

            }

            return View();
        }
        [HttpGet]
        public ActionResult add_jd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_jd(FormCollection form)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index");

            }

            return View();
        }
        [HttpGet]
        public ActionResult add_cust()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_cust(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                Customer a = new Customer();
                a.Cust_FirstName = form["firstname"];
                a.Cust_LastName = form["lastname"];
                a.Cust_Email = form["email"];
                a.Cust_Password = form["password"];
                a.Cust_Mobile = form["mobile"];
                a.Cust_JobDescription = form["job"];
                a.Cust_image = form["photo"];
                db.Customers.Add(a);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View();
        }
        public ActionResult deletepost(int id)
        {
            if(ModelState.IsValid)
            {
                List<Comment> a = new List<Comment>();
                a = db.Comments.Where(x => x.Post_ID == id).ToList();
                List<CurrentProject> b = new List<CurrentProject>();
                b = db.CurrentProjects.Where(x => x.Post_ID == id).ToList();
                List<PreProject> c = new List<PreProject>();
                c = db.PreProjects.Where(x => x.Post_ID == id).ToList();
                List<JdCurrentProject> d = new List<JdCurrentProject>();
                d = db.JdCurrentProjects.Where(x => x.Post_id == id).ToList();
                List<JdPreProject> e = new List<JdPreProject>();
                e = db.JdPreProjects.Where(x => x.Post_id == id).ToList();
                if(a.Count.Equals(0) && b.Count.Equals(0) && c.Count.Equals(0) && d.Count.Equals(0) && e.Count.Equals(0))
                {
                    Project p = new Project();
                    p = db.Projects.Where(x => x.Post_ID == id).SingleOrDefault();
                    db.Projects.Remove(p);
                    db.SaveChanges();
                    error = "null";
                    return RedirectToAction("Index");
                }
                else
                {
                    error = "Can Not Remove This Post";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult add_post()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_post(FormCollection form)
        {
            if(ModelState.IsValid)
            {
                Project p = new Project();
                p.Post_Description = form["postdes"];
                p.Cust_ID = int.Parse(Session["actorid"].ToString());
                p.Post_Time = DateTime.Now;
                p.is_admin = "true";
                db.Projects.Add(p);
                db.SaveChanges();
                error = "null";
                return RedirectToAction("Index");
            }
            return View();
        }
        public static int? a;
        [HttpGet]
        public ActionResult update_post(int? id)
        {
            if(id == null)
            {

            }
            else
            {
                a = id;
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult update_post(string postdes)
        {
            if(ModelState.IsValid)
            {
                Project s = db.Projects.Where(x => x.Post_ID == a).SingleOrDefault();
                s.Post_Description = postdes;
                db.Projects.SqlQuery("update Project set Post_Description = '" + postdes + "' where Post_ID = " + a + "");
                db.SaveChanges();
                error = "null";
                return RedirectToAction("Index");
            }
            
            return View();
        }
    }
}