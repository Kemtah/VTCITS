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
    public class BuildingController : Controller
    {
        private VtcContext db = new VtcContext();

        //
        // GET: /Building/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Buildings.ToList());
        }

        //
        // GET: /Building/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        //
        // GET: /Building/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Building/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Building building)
        {
            if (ModelState.IsValid)
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(building);
        }

        //
        // GET: /Building/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        //
        // POST: /Building/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(building);
        }

        //
        // GET: /Building/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        //
        // POST: /Building/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            db.Buildings.Remove(building);
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