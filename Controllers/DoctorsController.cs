using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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

        // Login
        // GET: Doctors/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Doctors/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(DOCTOR objUser)
        {

            using (Appointement_MVC_Entities db = new Appointement_MVC_Entities())
            {
                var obj = db.DOCTOR.Where(a => a.username.Equals(objUser.username) && a.password.Equals(objUser.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["doctorAMKA"] = obj.doctorAMKA.ToString();
                    Session["UserName"] = obj.username.ToString();
                    return RedirectToAction("DoctorMenu");
                }
            }
            ViewData["Error"] = "Please check your email and password!";
            return View(objUser);
        }

        // GET: Doctors
        public ActionResult DoctorMenu()
        {
            if (Session["doctorAMKA"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session["doctorAMKA"] = null;
            Session["UserName"] = null;
            Session.Abandon();
            return Redirect("../Home");
        }


        // GET: Index -> login
        public ActionResult Index()
        {
            return RedirectToAction("DoctorMenu");
        }


        // GET: Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doctorAMKA,username,name,surname,password,speciality,ADMIN_userid")] DOCTOR doctor)
        {
            if (ModelState.IsValid)
            {
                //int AdminID = (int)Session["admin"];


                try
                {
                    //doctor.ADMIN_userid = Convert.ToInt32(Session["admin"]);
                    db.DOCTOR.Add(doctor);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Login");
            }

            return View(doctor);
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
