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
            var appointments = db.APPOINTMENT.Include(a => a.DOCTOR).Include(a => a.PATIENT).OrderBy(s => s.date);

            if (Session["patientAMKA"] != null)
            {
                int amka = Convert.ToInt32(Session["patientAMKA"]);
                appointments = appointments.Where(x => x.PATIENT_patientAMKA == amka).OrderBy(s => s.date);
                return View(appointments.ToList());
            }
            else if (Session["doctorAMKA"] != null)
            {
                int amka = Convert.ToInt32(Session["doctorAMKA"]);
                appointments = appointments.Where(x => x.DOCTOR_doctorAMKA == amka).OrderBy(s => s.date);
                return View(appointments.ToList());
            }
            else if (Session["admin"] != null)
            {
                return View(appointments.ToList());
            }

            return Redirect("~/Home");
        }

        // GET: Appointments/Details/5
        public ActionResult Details(DateTime id)
        {
            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] != null)
            {
                return Redirect("~/Home");
            }

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
            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] != null)
            {
                return Redirect("~/Home");
            }
            ListParse();
            return View();
        }

        private void ListParse()
        {
            var doctors = db.DOCTOR.Select(s => new
                {
                    FullName = s.name + " " + s.surname,
                    amka = s.doctorAMKA
                }).ToList();

            var patients = db.PATIENT.Select(s => new
                {
                    FullName = s.name + " " + s.surname,
                    amka = s.patientAMKA
                }).ToList();

            ViewBag.DOCTOR_doctorAMKA = new SelectList(doctors, "amka", "FullName");
            ViewBag.PATIENT_patientAMKA = new SelectList(patients, "amka", "FullName");
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] APPOINTMENT appointment)
        {

            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] != null)
            {
                return Redirect("~/Home");
            }

            if ((appointment.date < DateTime.Today) || (appointment.date == DateTime.Today && appointment.startSlotTime <= DateTime.Now))
            {
                ViewData["Error"] = "This Day is not available!";
                ListParse();
                return View();
            }

            if (appointment.startSlotTime > Convert.ToDateTime("20:00:00") || appointment.startSlotTime < Convert.ToDateTime("09:00:00"))
            {
                ViewData["Error"] = "Please select hours where the Doctor is available";
                ListParse();
                return View();
            }

            appointment.endSlotTime = appointment.startSlotTime.AddHours(1);


            if (Session["patientAMKA"] != null)
            {
                appointment.PATIENT_patientAMKA = Convert.ToInt32(Session["patientAMKA"]);
            }
            else if (Session["doctorAMKA"] != null)
            {
                appointment.DOCTOR_doctorAMKA = Convert.ToInt32(Session["doctorAMKA"]);
            }


            if (ModelState.IsValid)
            {
                if (db.APPOINTMENT.Where(x =>
                        x.startSlotTime == appointment.startSlotTime && x.date == appointment.date &&
                        x.DOCTOR_doctorAMKA == appointment.DOCTOR_doctorAMKA).FirstOrDefault() == null)
                {
                    db.APPOINTMENT.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewData["Error"] = "Time unavailable";
                ListParse();
                return View();

            }
            ViewData["Error"] = "Data Inputs not accepted";
            ListParse();
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] == null)
            {
                return Redirect("~/Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPOINTMENT appointment = db.APPOINTMENT.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ListParse();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] APPOINTMENT appointment)
        {

            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] == null)
            {
                return Redirect("~/Home");
            }

            if ((appointment.date < DateTime.Today) || (appointment.date == DateTime.Today && appointment.startSlotTime <= DateTime.Now))
            {
                ViewData["Error"] = "This Day is not available!";
                ListParse();
                return View();
            }

            if (appointment.startSlotTime > Convert.ToDateTime("20:00:00") || appointment.startSlotTime < Convert.ToDateTime("09:00:00"))
            {
                ViewData["Error"] = "Please select hours where the Doctor is available";
                ListParse();
                return View();
            }

            appointment.endSlotTime = appointment.startSlotTime.AddHours(1);

            if (Session["patientAMKA"] != null)
            {
                appointment.PATIENT_patientAMKA = Convert.ToInt32(Session["patientAMKA"]);
            }
            else if (Session["doctorAMKA"] != null)
            {
                appointment.DOCTOR_doctorAMKA = Convert.ToInt32(Session["doctorAMKA"]);
            }

            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Error"] = "Data Inputs not accepted";
            ListParse();
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] == null)
            {
                return Redirect("~/Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPOINTMENT appointment = db.APPOINTMENT.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            if (Session["patientAMKA"] == null && Session["doctorAMKA"] == null && Session["admin"] == null)
            {
                return Redirect("~/Home");
            }

            APPOINTMENT appointment = db.APPOINTMENT.Find(id);
            db.APPOINTMENT.Remove(appointment);
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
