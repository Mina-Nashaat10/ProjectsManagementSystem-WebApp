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
    public class CommentController : Controller
    {
        PMSDBEntities d = new PMSDBEntities();

        // GET: comments
        public ActionResult Index()
        {

            var comments = d.Comments.ToList();
            return View(comments.ToList());
        }

        // GET: comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = d.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}


        //// POST: comments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        //// GET: comments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = d.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //ViewBag.pid = new SelectList(d.pos, "pid", "pdesc", comment.pid);
        //    return View(comment);
        //}

        //// POST: comments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "coid,cdescription,pid")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        d.Entry(comment).State = EntityState.Modified;
        //        d.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    //ViewBag.pid = new SelectList(d., "pid", "pdesc", comment.Comment_ID);
        //    return View(comment);
        //}

        //// GET: comments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    comment comment = d.comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}

        //// POST: comments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    comment comment = d.comments.Find(id);
        //    d.comments.Remove(comment);
        //    d.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        d.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        [HttpPost]

        public ActionResult Add_Comment(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                Comment c = new Comment();
                c.Comment_Message = form["cdescription"];
                c.Post_ID = int.Parse(form["pid"]);
                c.PM_ID = int.Parse(Session["actorid"].ToString());
                int y = int.Parse(form["pid"]);
                var s = d.Projects.Where(x => x.Post_ID == y).SingleOrDefault();
                int f = s.Cust_ID;
                c.Cust_ID = f;

                d.Comments.Add(c);
                d.SaveChanges();
                return RedirectToAction("show_comments");
            }
            return View();
        }



        public ActionResult show_comments()
        {

            int id = int.Parse(Session["id"].ToString());


            var comment = d.Comments.Where(x => x.Post_ID == id).ToList();

            return View(comment);

            /*

            int id = int.Parse(Session["id"].ToString());
            var comments = d.comments.Include(c => c.pos);
            return View(comments.Where(x => x.pid == id).ToListAsync());
            */
        }

        //public ActionResult showcomments()
        //{

        //    int id = int.Parse(Session["id"].ToString());


        //    if (id == -1)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    comment comment = d.comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}
        /*

        int id = int.Parse(Session["id"].ToString());
        var comments = d.comments.Include(c => c.pos);
        return View(comments.Where(x => x.pid == id).ToListAsync());
        */




        /*
         [HttpPost]

         public ActionResult Add_Comment([Bind(Include = "coid,cdescription,pid")] comment comment)
         {
             if (ModelState.IsValid)
             {

                 comment.coid = 8;

                 comment.pid = 2;

                 d.comments.Add(comment);
                 d.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.pid = new SelectList(d.pos, "pid", "pdesc", comment.pid);
             return View(comment);
         }
         */
    }
}