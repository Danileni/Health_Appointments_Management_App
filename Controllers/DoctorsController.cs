using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthAppointmentsManagement.Models;

namespace HealthAppointmentsManagement.Controllers
{
    public class DoctorsController : Controller
    {
        private Appointement_MVC_Entities db = new Appointement_MVC_Entities();

        // GET: Doctors
        public ActionResult Index()
        {
            var dOCTOR = db.DOCTOR.Include(d => d.ADMIN);
            return View(dOCTOR.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCTOR dOCTOR = db.DOCTOR.Find(id);
            if (dOCTOR == null)
            {
                return HttpNotFound();
            }
            return View(dOCTOR);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.ADMIN_userid = new SelectList(db.ADMIN, "userid", "username");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doctorAMKA,username,name,password,surname,specialty,ADMIN_userid")] DOCTOR dOCTOR)
        {
            if (ModelState.IsValid)
            {
                db.DOCTOR.Add(dOCTOR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ADMIN_userid = new SelectList(db.ADMIN, "userid", "username", dOCTOR.ADMIN_userid);
            return View(dOCTOR);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCTOR dOCTOR = db.DOCTOR.Find(id);
            if (dOCTOR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADMIN_userid = new SelectList(db.ADMIN, "userid", "username", dOCTOR.ADMIN_userid);
            return View(dOCTOR);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "doctorAMKA,username,name,password,surname,specialty,ADMIN_userid")] DOCTOR dOCTOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCTOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ADMIN_userid = new SelectList(db.ADMIN, "userid", "username", dOCTOR.ADMIN_userid);
            return View(dOCTOR);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCTOR dOCTOR = db.DOCTOR.Find(id);
            if (dOCTOR == null)
            {
                return HttpNotFound();
            }
            return View(dOCTOR);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DOCTOR dOCTOR = db.DOCTOR.Find(id);
            db.DOCTOR.Remove(dOCTOR);
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
    }
}
