using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Diagnostics;


namespace WebApplication2.Controllers
{
    public class PMController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();

        //public ActionResult Chart()
        //{
        //    ArrayList xvalue = new ArrayList();
        //    ArrayList yvalue = new ArrayList();

        //    List<Previous_Projects> evl = new List<Previous_Projects>();
        //    List<Previous_Projects> edate = new List<Previous_Projects>();
        //    evl = (List<Previous_Projects>)db.Previous_Projects.AsEnumerable().Select(s => s.evaluation);
        //    edate = (List<Previous_Projects>)db.Previous_Projects.AsEnumerable().Select(s => s.EndData);

        //    for (int i = 0; i < evl.Count(); i++)
        //    {
        //        if (i == 0)
        //        {
        //            xvalue[i] = evl[i].evaluation;
        //            yvalue[i] = edate[i].EndData;
        //        }
        //        else
        //        {
        //            for (int j = 0; j < i; j++)
        //            {
        //                if (yvalue[j] == yvalue[i])
        //                {

        //                }
        //            }
        //        }
        //    }
        //    return View();
        //}
        public ActionResult PreviousProjectsandcurrent()
        {
            using (PMSDBEntities p = new PMSDBEntities())
            {
                if (Session["actorid"] != null)
                {
                    // FOR Previous Pojects
                    int id = int.Parse(Session["actorid"].ToString());
                    var pro = p.PreProjects.Where(x => x.PM_ID == id).ToList();
                    List<TeamLeader> teamleader = new List<TeamLeader>();
                    List<Customer> cust = new List<Customer>();
                    for (int i = 0; i < pro.Count(); i++)
                    {
                        int s = (int)pro[i].TL_ID;
                        var t = p.TeamLeaders.Where(v => v.TL_ID == s).First();
                        TeamLeader tl = new TeamLeader();
                        tl.TL_FirstName = t.TL_FirstName;
                        tl.TL_LastName = t.TL_LastName;
                        teamleader.Add(tl);

                        int k = (int)pro[i].Cust_ID;
                        var d = p.Customers.Where(b => b.Cust_ID == k).First();
                        Customer r = new Customer();
                        r.Cust_FirstName = d.Cust_FirstName;
                        r.Cust_LastName = d.Cust_LastName;
                        cust.Add(r);
                    }
                    ViewBag.team = teamleader;
                    ViewBag.customers = cust;
                    ViewBag.allprojects = pro;


                    int[] arr = new int[pro.Count()];
                    int q = 0;
                    foreach (var item in pro)
                    {
                        arr[q] = item.Post_ID;
                        q++;
                    }

                    int[,] jd = new int[arr.Length, 7];
                    int[] count = new int[arr.Length];
                    int u = 0;
                    for (int j = 0; j < arr.Count(); j++)
                    {

                        int k = arr[j];
                        var e = db.JdPreProjects.Where(w => w.Post_id == k).ToList();
                        for (int o = 0; o < e.Count(); o++)
                        {
                            JdPreProject d = new JdPreProject();
                            d = e[o];
                            int ee = d.JD_id;
                            jd[j, o] = ee;
                            u++;
                        }
                        count[j] = u;
                        u = 0;
                    }
                    String[,] namesofjds = new string[jd.Length, 10];
                    for (int m = 0; m < arr.Length; m++)
                    {
                        for (int f = 0; f < count[m]; f++)
                        {
                            int rr = jd[m, f];
                            var n = db.JuniorDevelopers.Where(g => g.JD_ID == rr).Single();
                            namesofjds[m, f] = n.JD_FirstName + " " + n.JD_LastName;
                        }
                    }
                    ViewBag.namejds = namesofjds;
                    ViewBag.countjds = count;
                    int cc = int.Parse(Session["actorid"].ToString());
                    var gg = db.ProjectManagers.Where(f => f.PM_id == cc).SingleOrDefault();
                    ViewBag.inf = gg;

                    int ka = int.Parse(Session["actorid"].ToString());
                    var uu = db.Notifications.Where(h => h.Actor2_name.Equals("PM") && h.Person2_Id == ka).ToList();
                    ViewBag.allnotificationpm = uu;
                    string[] names = new string[uu.Count()];
                    int ii = 0;
                    foreach (var item in uu)
                    {
                        int bb = uu[ii].Person1_Id;
                        var f = db.Customers.Where(z => z.Cust_ID == bb).First();
                        names[ii] = f.Cust_FirstName + " " + f.Cust_LastName;
                        ii++;
                    }
                    ViewBag.allcustomers = names;
                    string[] names1 = new string[uu.Count()];
                    ii = 0;
                    foreach (var item in uu)
                    {
                        int bb = uu[ii].Post_ID;
                        var f = db.Projects.Where(z => z.Post_ID == bb).First();
                        names1[ii] = f.Post_Description;
                        ii++;
                    }
                    ViewBag.allposts = names1;
                    //FOR Current Projects
                    int id1 = int.Parse(Session["actorid"].ToString());
                    var currentpro = db.CurrentProjects.Where(x => x.PM_ID == id1).ToList();
                    List<TeamLeader> teamleader1 = new List<TeamLeader>();
                    List<Customer> cust1 = new List<Customer>();
                    for (int i = 0; i < currentpro.Count(); i++)
                    {
                        if (currentpro[i].TL_ID != null)
                        {
                            int s = (int)currentpro[i].TL_ID;
                            var t = db.TeamLeaders.Where(v => v.TL_ID == s).First();
                            TeamLeader tl = new TeamLeader();
                            tl.TL_FirstName = t.TL_FirstName;
                            tl.TL_LastName = t.TL_LastName;
                            teamleader1.Add(tl);
                        }
                        else
                        {
                            TeamLeader l = new TeamLeader();
                            l.TL_FirstName = "NOT";
                            l.TL_LastName = "Assign";
                            teamleader1.Add(l);
                        }

                        int k = (int)currentpro[i].Cust_ID;
                        var d = db.Customers.Where(b => b.Cust_ID == k).First();
                        Customer r = new Customer();
                        r.Cust_FirstName = d.Cust_FirstName;
                        r.Cust_LastName = d.Cust_LastName;
                        cust1.Add(r);
                    }
                    ViewBag.team1 = teamleader1;
                    ViewBag.customers1 = cust1;
                    ViewBag.allprojects1 = currentpro;

                    int[] arr1 = new int[currentpro.Count()];
                    int q1 = 0;
                    foreach (var item in currentpro)
                    {
                        arr1[q1] = item.Post_ID;
                        q1++;
                    }

                    int[,] jd1 = new int[arr1.Length, 7];
                    int[] count1 = new int[arr1.Length];
                    int u1 = 0;
                    for (int j = 0; j < arr1.Count(); j++)
                    {

                        int k = arr1[j];
                        var e = db.JdCurrentProjects.Where(w => w.Post_id == k).ToList();
                        for (int o = 0; o < e.Count(); o++)
                        {
                            JdCurrentProject d = new JdCurrentProject();
                            d = e[o];
                            int ee = d.Jd_id;
                            jd1[j, o] = ee;
                            u1++;
                        }
                        count1[j] = u1;
                        u1 = 0;
                    }
                    String[,] namesofjds1 = new string[jd1.Length, 10];
                    for (int m = 0; m < arr1.Length; m++)
                    {
                        for (int f = 0; f < count1[m]; f++)
                        {
                            int rr = jd1[m, f];
                            var n = db.JuniorDevelopers.Where(g => g.JD_ID == rr).Single();
                            namesofjds1[m, f] = n.JD_FirstName + " " + n.JD_LastName;
                        }
                    }
                    ViewBag.namejds1 = namesofjds1;
                    ViewBag.countjds1 = count1;

                    // get teamLeaders Names that sends Notifications 
                    List<String> teamLeadersNames = new List<String>();
                    int pmId = int.Parse(Session["actorid"].ToString());
                    var pmNotifications = db.Notifications.Where(i => i.Actor1_Name == "TL" && i.Actor2_name == "PM" && i.Person2_Id == pmId).ToList();
                    foreach(Notification n in pmNotifications)
                    {
                        var teamLeaderInstance = db.TeamLeaders.Where(i => i.TL_ID == n.Person1_Id).Single();
                        teamLeadersNames.Add(teamLeaderInstance.TL_FirstName + " " + teamLeaderInstance.TL_LastName);
                    }
                    ViewBag.teamLeadersNames = teamLeadersNames;

                    //get juniorDevelopers Names that Sends Notifications
                    List<String> juniorDeveloperNames = new List<String>();
                    pmNotifications = db.Notifications.Where(i => i.Actor1_Name == "JD" && i.Actor2_name == "PM" && i.Person2_Id == pmId).ToList();
                    foreach (Notification n in pmNotifications)
                    {
                        var juniorDeveloperInstance = db.JuniorDevelopers.Where(i => i.JD_ID == n.Person1_Id).Single();
                        juniorDeveloperNames.Add(juniorDeveloperInstance.JD_FirstName + " " + juniorDeveloperInstance.JD_LastName);
                    }
                    ViewBag.juniorDevelopersNames = juniorDeveloperNames;
                }
            }
            return View();
        }
        public ActionResult search1()
        {
            var s = db.TeamLeaders.ToList();
            ViewBag.all = s;
            return View();
        }
        public ActionResult search2()
        {
            var s = db.JuniorDevelopers.ToList();
            ViewBag.all = s;
            return View();
        }
        public ActionResult reject(int id, int actor)
        {
            if (ModelState.IsValid)
            {
                Notification n = new Notification();
                // get PM info
                int currentId = int.Parse(Session["actorid"].ToString());
                n.Person1_Id = currentId;
                int q = int.Parse(Session["roleid"].ToString());
                var x = db.Roles.Where(b => b.rid == q).SingleOrDefault();
                n.Actor1_Name = x.rname;


                n.Person2_Id = id;
                n.Actor2_name = "Customer";
                n.Date = DateTime.Now.ToString();
                n.Post_ID = actor;
                n.Message = "Sorry, I'm very busy...";
                db.Notifications.Add(n);
                var s = db.Notifications.Where(f => f.Actor1_Name.Equals("Customer") && f.Actor2_name.Equals("PM") && f.Person1_Id == id && f.Person2_Id == currentId && f.Post_ID == actor).First();
                db.Notifications.Remove(s);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
        public ActionResult accept(int id, int actor)
        {
            if (ModelState.IsValid)
            {
                var t = db.Projects.Where(f => f.Post_ID == actor).First();
                int currentId = int.Parse(Session["actorid"].ToString());
                t.PM_ID = currentId;
                db.SaveChanges();

                var b = db.Projects.Where(m => m.Post_ID == actor).First();
                CurrentProject c = new CurrentProject();
                c.Post_ID = b.Post_ID;
                c.Post_Description = b.Post_Description;
                c.PM_ID = currentId;
                c.Cust_ID = b.Cust_ID;
                c.Start_Date = DateTime.Now.ToString();
                c.DeadLine = "21/8/2024";
                db.CurrentProjects.Add(c);
                db.SaveChanges();


                var e = db.Notifications.Where(h => h.Actor1_Name.Equals("Customer") && h.Actor2_name.Equals("PM") && h.Person1_Id == id && h.Person2_Id == currentId && h.Post_ID == actor).First();
                db.Notifications.Remove(e);
                db.SaveChanges();
                var q = db.Projects.Where(k => k.Post_ID == actor).First();
                Notification n = new Notification();
                int u = int.Parse(Session["actorid"].ToString());
                n.Person1_Id = u;
                n.Actor1_Name = "PM";
                n.Person2_Id = q.Cust_ID;
                n.Actor2_name = "Customer";
                n.Message = "I Agree";
                n.Date = DateTime.Now.ToString();
                n.Post_ID = actor;
                db.Notifications.Add(n);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
        public static int teamLeaderId;
        [HttpGet]
        public ActionResult sendNotificationToTeamLeader(int id)
        {
            teamLeaderId = id;
            int pmId = int.Parse(Session["actorid"].ToString());
            var pmProjects = db.Projects.Where(i => i.PM_ID == pmId).ToList();
            ViewBag.allprojects = pmProjects;
            return View();
        }
        [HttpPost]
        public ActionResult sendNotificationToTeamLeader(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                // get Project Manager Info
                int pmId = int.Parse(Session["actorid"].ToString());
                String pmActorName = "PM";

                // get Team Leader Info
                int tlId = teamLeaderId;
                Debug.WriteLine("teamleaderid = " + teamLeaderId);


                Notification newNotification = new Notification();
                newNotification.Person1_Id = pmId;
                newNotification.Actor1_Name = pmActorName;
                newNotification.Person2_Id = tlId;
                newNotification.Actor2_name = "TL";
                newNotification.Message = form["message"];
                newNotification.Date = DateTime.Now.ToString();
                string postDes = form["postdes"];
                var myProject = db.Projects.Where(i => i.Post_Description.Equals(postDes)).First();
                newNotification.Post_ID = myProject.Post_ID;
                db.Notifications.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
        public ActionResult deleteNotificationFromTL(int pmId, int tlId, int postId)
        {
            if (ModelState.IsValid)
            {
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == tlId && i.Actor1_Name == "TL" && i.Person2_Id == pmId && i.Actor2_name == "PM" && i.Post_ID == postId).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
        public static int juniorDeveloperId;
        [HttpGet]
        public ActionResult sendNotificationToJuniorDeveloper(int? jdId)
        {
            if (ModelState.IsValid)
            {
                juniorDeveloperId = (int)jdId;
                int pmId = int.Parse(Session["actorid"].ToString());
                var pmProjects = db.Projects.Where(i => i.PM_ID == pmId).ToList();
                ViewBag.allprojects = pmProjects;
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult sendNotificationToJuniorDeveloper(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                // get Project Manager Info
                int pmId = int.Parse(Session["actorid"].ToString());
                String pmActorName = "PM";

                // get Team Leader Info
                int jdId = juniorDeveloperId;
                Debug.WriteLine("teamleaderid = " + teamLeaderId);


                Notification newNotification = new Notification();
                newNotification.Person1_Id = pmId;
                newNotification.Actor1_Name = pmActorName;
                newNotification.Person2_Id = jdId;
                newNotification.Actor2_name = "JD";
                newNotification.Message = form["message"];
                newNotification.Date = DateTime.Now.ToString();
                string postDes = form["postdes"];
                var myProject = db.Projects.Where(i => i.Post_Description.Equals(postDes)).First();
                newNotification.Post_ID = myProject.Post_ID;
                db.Notifications.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
        public ActionResult deleteNotificationFromJD(int pmId, int jdId, int postId)
        {
            if (ModelState.IsValid)
            {
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == jdId && i.Actor1_Name == "JD" && i.Person2_Id == pmId && i.Actor2_name == "PM" && i.Post_ID == postId).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "PM");
            }
            return View();
        }
    }
}