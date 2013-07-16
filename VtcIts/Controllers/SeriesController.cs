using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VtcIts.Filters;
using VtcIts.Models;

namespace VtcIts.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Admin")]
    public class SeriesController : Controller
    {
        private VtcContext db = new VtcContext();

        //
        // GET: /Series/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Series.ToList());
        }

        //
        // GET: /Series/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        //
        // GET: /Series/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Series/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Series series)
        {
            if (ModelState.IsValid)
            {
                db.Series.Add(series);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(series);
        }

        //
        // GET: /Series/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        //
        // POST: /Series/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Series series)
        {
            if (ModelState.IsValid)
            {
                db.Entry(series).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(series);
        }

        //
        // GET: /Series/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        //
        // POST: /Series/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Series series = db.Series.Find(id);
            db.Series.Remove(series);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}