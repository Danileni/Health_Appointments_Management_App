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
    public class PatientsController : Controller
    {
        private Appointement_MVC_Entities db = new Appointement_MVC_Entities();

        // Login
        // GET: Patients/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Patients/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PATIENT objUser)
        {

            using (Appointement_MVC_Entities db = new Appointement_MVC_Entities())
            {
                var obj = db.PATIENT.Where(a => a.patientAMKA.Equals(objUser.patientAMKA) && a.password.Equals(objUser.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserAMKA"] = obj.patientAMKA.ToString();
                    Session["UserName"] = obj.name.ToString();
                    return RedirectToAction("Schedule");
                }
            }
            ViewData["Error"] = "Please check your email and password!";
            return View(objUser);
        }

        // GET: Patients/Schedule
        public ActionResult Schedule()
        {
            if (Session["UserAMKA"] == null)
            {
                return RedirectToAction("Login");

            }
            return View();
        }

        //Logout

        public ActionResult Logout()
        {
            Session["UserAMKA"] = null;
            Session["UserName"] = null;
            Session.Abandon();
            return Redirect("../Home");
        }

        // GET: Patients
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "patientAMKA,userid,username,name,surname,password")] PATIENT patient)
        {
            if (ModelState.IsValid)
            {
                db.PATIENT.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(patient);
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
