using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class PostController : Controller
    {
        PMSDBEntities db = new PMSDBEntities();

        // GET: pos
        public ActionResult Index()
        {
            var pos = db.Projects.Where(f=>f.PM_ID == null).ToList();
            return View(pos);
        }

        // GET: pos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project pos = db.Projects.Find(id);
            if (pos == null)
            {
                return HttpNotFound();
            }
            return View(pos);
        }

        // GET: pos/Create
        public ActionResult Create()
        {
            ViewBag.Cid = new SelectList(db.Customers, "Cid", "Cname");
            return View();
        }

        // POST: pos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pid,pdesc,Cid,timess")] Project pos)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(pos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cid = new SelectList(db.Customers, "Cid", "Cname",pos.Cust_ID );
            return View(pos);
        }

        // GET: pos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project pos = db.Projects.Find(id);
            if (pos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cid = new SelectList(db.Customers, "Cid", "Cname", pos.Cust_ID);
            return View(pos);
        }

        // POST: pos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,pdesc,Cid,timess")] Project pos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cid = new SelectList(db.Customers, "Cid", "Cname", pos.Cust_ID);
            return View(pos);
        }

        // GET: pos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project pos = db.Projects.Find(id);
            if (pos == null)
            {
                return HttpNotFound();
            }
            return View(pos);
        }

        // POST: pos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Project pos = db.Projects.Find(id);
            db.Projects.Remove(pos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult deatails(int? id)
        {
            return View();
        }




        [HttpPost]

        public ActionResult Add_post(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if(form["pdesc"].Equals(""))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Project p = new Project();
                    p.Post_Description = form["pdesc"];
                    p.Cust_ID = int.Parse(Session["actorid"].ToString());
                    p.Post_Time = DateTime.Now;
                    p.is_admin = "false";
                    db.Projects.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult showcomments(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


             Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);


        }
    }
}