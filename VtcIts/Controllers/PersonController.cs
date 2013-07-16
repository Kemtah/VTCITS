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
    public class PersonController : Controller
    {
        private VtcContext db = new VtcContext();

        //
        // GET: /Participant/

        [AllowAnonymous]
        public ActionResult Index()
        {
            var people = db.People.Include(p => p.Building).Include(p => p.Location);
            return View(people.ToList());
        }

        //
        // GET: /Participant/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // GET: /Participant/Create

        public ActionResult Create()
        {
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        //
        // POST: /Participant/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", person.BuildingId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", person.LocationId);
            return View(person);
        }

        //
        // GET: /Participant/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", person.BuildingId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", person.LocationId);
            return View(person);
        }

        //
        // POST: /Participant/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", person.BuildingId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", person.LocationId);
            return View(person);
        }

        //
        // GET: /Participant/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // POST: /Participant/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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