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
    public class KemtahTechController : Controller
    {
        private VtcContext db = new VtcContext();

        //
        // GET: /KemtahTech/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.KemtahTeches.ToList());
        }

        //
        // GET: /KemtahTech/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            KemtahTech kemtahtech = db.KemtahTeches.Find(id);
            if (kemtahtech == null)
            {
                return HttpNotFound();
            }
            return View(kemtahtech);
        }

        //
        // GET: /KemtahTech/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /KemtahTech/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KemtahTech kemtahtech)
        {
            if (ModelState.IsValid)
            {
                db.KemtahTeches.Add(kemtahtech);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kemtahtech);
        }

        //
        // GET: /KemtahTech/Edit/5

        public ActionResult Edit(int id = 0)
        {
            KemtahTech kemtahtech = db.KemtahTeches.Find(id);
            if (kemtahtech == null)
            {
                return HttpNotFound();
            }
            return View(kemtahtech);
        }

        //
        // POST: /KemtahTech/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KemtahTech kemtahtech)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kemtahtech).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kemtahtech);
        }

        //
        // GET: /KemtahTech/Delete/5

        public ActionResult Delete(int id = 0)
        {
            KemtahTech kemtahtech = db.KemtahTeches.Find(id);
            if (kemtahtech == null)
            {
                return HttpNotFound();
            }
            return View(kemtahtech);
        }

        //
        // POST: /KemtahTech/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KemtahTech kemtahtech = db.KemtahTeches.Find(id);
            db.KemtahTeches.Remove(kemtahtech);
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