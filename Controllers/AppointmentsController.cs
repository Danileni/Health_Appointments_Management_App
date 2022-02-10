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
    public class AppointmentsController : Controller
    {
        private Appointement_MVC_Entities db = new Appointement_MVC_Entities();

        // GET: Appointments
        public ActionResult Index()
        {
            var aPPOINTMENT = db.APPOINTMENT.Include(a => a.DOCTOR).Include(a => a.PATIENT);
            return View(aPPOINTMENT.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPOINTMENT aPPOINTMENT = db.APPOINTMENT.Find(id);
            if (aPPOINTMENT == null)
            {
                return HttpNotFound();
            }
            return View(aPPOINTMENT);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.DOCTOR, "doctorAMKA", "username");
            ViewBag.PATIENT_patientAMKA = new SelectList(db.PATIENT, "patientAMKA", "userid");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] APPOINTMENT aPPOINTMENT)
        {
            if (ModelState.IsValid)
            {
                db.APPOINTMENT.Add(aPPOINTMENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.DOCTOR, "doctorAMKA", "username", aPPOINTMENT.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.PATIENT, "patientAMKA", "userid", aPPOINTMENT.PATIENT_patientAMKA);
            return View(aPPOINTMENT);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPOINTMENT aPPOINTMENT = db.APPOINTMENT.Find(id);
            if (aPPOINTMENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.DOCTOR, "doctorAMKA", "username", aPPOINTMENT.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.PATIENT, "patientAMKA", "userid", aPPOINTMENT.PATIENT_patientAMKA);
            return View(aPPOINTMENT);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] APPOINTMENT aPPOINTMENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aPPOINTMENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.DOCTOR, "doctorAMKA", "username", aPPOINTMENT.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.PATIENT, "patientAMKA", "userid", aPPOINTMENT.PATIENT_patientAMKA);
            return View(aPPOINTMENT);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPOINTMENT aPPOINTMENT = db.APPOINTMENT.Find(id);
            if (aPPOINTMENT == null)
            {
                return HttpNotFound();
            }
            return View(aPPOINTMENT);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            APPOINTMENT aPPOINTMENT = db.APPOINTMENT.Find(id);
            db.APPOINTMENT.Remove(aPPOINTMENT);
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
