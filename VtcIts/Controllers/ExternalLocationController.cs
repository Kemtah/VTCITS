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
    public class ExternalLocationController : Controller
    {
        private VtcContext db = new VtcContext();

        //
        // GET: /ExternalLocation/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.ExternalLocations.ToList());
        }

        //
        // GET: /ExternalLocation/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            ExternalLocation externallocation = db.ExternalLocations.Find(id);
            if (externallocation == null)
            {
                return HttpNotFound();
            }
            return View(externallocation);
        }

        //
        // GET: /ExternalLocation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ExternalLocation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExternalLocation externallocation)
        {
            if (ModelState.IsValid)
            {
                db.ExternalLocations.Add(externallocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(externallocation);
        }

        //
        // GET: /ExternalLocation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ExternalLocation externallocation = db.ExternalLocations.Find(id);
            if (externallocation == null)
            {
                return HttpNotFound();
            }
            return View(externallocation);
        }

        //
        // POST: /ExternalLocation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExternalLocation externallocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(externallocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(externallocation);
        }

        //
        // GET: /ExternalLocation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ExternalLocation externallocation = db.ExternalLocations.Find(id);
            if (externallocation == null)
            {
                return HttpNotFound();
            }
            return View(externallocation);
        }

        //
        // POST: /ExternalLocation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExternalLocation externallocation = db.ExternalLocations.Find(id);
            db.ExternalLocations.Remove(externallocation);
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