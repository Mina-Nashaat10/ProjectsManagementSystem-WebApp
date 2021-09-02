using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TLController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();
        public ActionResult PreviousProjectsandcurrent()
        {
            using (PMSDBEntities p = new PMSDBEntities())
            {
                if (Session["actorid"] != null)
                {
                    // FOR Previous Pojects
                    int id = int.Parse(Session["actorid"].ToString());
                    var pro = p.PreProjects.Where(x => x.TL_ID == id).ToList();
                    List<ProjectManager> teamleader = new List<ProjectManager>();
                    List<Customer> cust = new List<Customer>();
                    for (int i = 0; i < pro.Count(); i++)
                    {
                        int s = (int)pro[i].PM_ID;
                        var t = p.ProjectManagers.Where(v => v.PM_id == s).First();
                        ProjectManager tl = new ProjectManager();
                        tl.PM_FirstName = t.PM_FirstName;
                        tl.PM_LastName = t.PM_LastName;
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
                    var gg = db.TeamLeaders.Where(f => f.TL_ID == cc).SingleOrDefault();
                    ViewBag.inf = gg;



                    //FOR Current Projects
                    int id1 = int.Parse(Session["actorid"].ToString());
                    var currentpro = db.CurrentProjects.Where(x => x.TL_ID == id1).ToList();
                    List<ProjectManager> teamleader1 = new List<ProjectManager>();
                    List<Customer> cust1 = new List<Customer>();
                    for (int i = 0; i < currentpro.Count(); i++)
                    {
                        int s = (int)currentpro[i].PM_ID;
                        var t = db.ProjectManagers.Where(v => v.PM_id == s).First();
                        ProjectManager tl = new ProjectManager();
                        tl.PM_FirstName = t.PM_FirstName;
                        tl.PM_LastName = t.PM_LastName;
                        teamleader1.Add(tl);

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

                    int kk = int.Parse(Session["actorid"].ToString());
                    var ss = db.Notifications.Where(w => w.Actor2_name.Equals("TL") && w.Person2_Id == kk).ToList();
                    ViewBag.allnottls = ss;

                    // get my current notification
                    int pmId = int.Parse(Session["actorid"].ToString());
                    List<Notification> myNotification = new List<Notification>();
                    myNotification = db.Notifications.Where(i => i.Person2_Id == pmId && i.Actor2_name == "TL").ToList();
                    ViewBag.allNotificationForTL = myNotification;

                    //get project mangers names that send these notifications
                    List<String> projectManagersNames = new List<string>();
                    ProjectManager pm;

                    // get project content for these notifications
                    List<Project> notificationProjects = new List<Project>();
                    for(int i=0;i<myNotification.Count;i++)
                    {
                        // get team leader name for this notification
                        pm = new ProjectManager();
                        Debug.WriteLine("TL = " + myNotification[i].Person2_Id);
                        int projectManagerId = (int)myNotification[i].Person1_Id;
                        pm = db.ProjectManagers.Where(s => s.PM_id == projectManagerId).Single();
                        projectManagersNames.Add(pm.PM_FirstName + " "+pm.PM_LastName);
                        int PostId = (int)myNotification[i].Post_ID;
                        notificationProjects.Add(db.Projects.Where(a => a.Post_ID == PostId).FirstOrDefault());
                    }
                    ViewBag.projectManagersNames = projectManagersNames;
                    ViewBag.posts = notificationProjects;
                    
                }
            }
            return View();
        }
        public ActionResult search()
        {
            var s = db.JuniorDevelopers.ToList();
            ViewBag.all = s;
            return View();
        }
        public ActionResult allpmforsendemail()
        {
            var pms = db.ProjectManagers.ToList();
            ViewBag.allpms = pms;
            return View();
        }
        public static int? a;
        public static string error1 = "null";
        [HttpGet]
        public ActionResult sendemail(int? id)
        {
            if (id != null)
                a = id;
            return View();
        }
        
        [HttpPost]
        public ActionResult sendemail()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string smtpAddress = "smtp.gmail.com";
                    int portNumber = 587;
                    bool enableSSL = true;


                    int x = int.Parse(Session["actorid"].ToString());
                    var y = db.TeamLeaders.Where(a => a.TL_ID == x).First();
                    string emailFrom = y.TL_Email;
                    string password = y.TL_Password;

                    var g = db.ProjectManagers.Where(v => v.PM_id == a).First();
                    string emailTo = g.PM_Email;

                    string subject = Request.Form["Subject"];
                    string body = Request.Form["Body"];

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFrom);
                        mail.To.Add(emailTo);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;
                        // Can set to false, if you are sending pure text.

                        //mail.Attachments.Add(new Attachment("C:\\n.txt"));

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            smtp.Credentials = new NetworkCredential(emailFrom, password);
                            smtp.EnableSsl = enableSSL;
                            smtp.Send(mail);
                        }
                    }
                    return RedirectToAction("PreviousProjectsandcurrent");
                }
                catch (Exception)
                {
                    ViewBag.er = "Can Not Send E-mail Becuse username or password invalid";
                }
            }
            else
            {
                ViewBag.er = "Error!";
                return View();
            }
            return View();
        }
        public ActionResult rejectPmRequest(int pmId , int postId)
        {
            if(ModelState.IsValid)
            {
                Debug.WriteLine("PmID = " + pmId);
                int tlId = int.Parse(Session["actorid"].ToString());
                String actortlName = "TL";

                String actorPmName = "PM";
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == pmId && i.Actor1_Name == actorPmName && i.Person2_Id == tlId && i.Actor2_name == actortlName).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();

                Notification newNotification = new Notification();
                newNotification.Person1_Id = tlId;
                newNotification.Actor1_Name = actortlName;

                newNotification.Person2_Id = pmId;
                newNotification.Actor2_name = actorPmName;

                newNotification.Message = "Sorry, I'm very busy...";
                newNotification.Date = DateTime.Now.ToString();
                newNotification.Post_ID = (int) postId;
                db.Notifications.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("PreviousProjectsandcurrent", "TL");
            }
            return View();
        }
        public ActionResult acceptPmRequest(int pmId, int postId)
        {
            if(ModelState.IsValid)
            {
                var currentProject = db.CurrentProjects.Where(i => i.Post_ID == postId && i.PM_ID == pmId).First();
                currentProject.TL_ID = int.Parse(Session["actorid"].ToString());
                db.SaveChanges();

                int tlId = int.Parse(Session["actorid"].ToString());
                var oldNotification = db.Notifications.Where(i => i.Person1_Id == pmId && i.Actor1_Name == "PM" && i.Person2_Id == tlId && i.Actor2_name == "TL" && i.Post_ID == postId).First();
                db.Notifications.Remove(oldNotification);
                db.SaveChanges();

                return RedirectToAction("PreviousProjectsandcurrent", "TL");
            }
            return View();
        }
    }
    
}