﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLAS.Models;
using PagedList;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace TLAS.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewData["compartments"] = db.Compartments.ToList();
            ViewBag.CurrentSort = sortOrder; // added new
            ViewBag.IdSortParm = sortOrder == "id" ? "id_desc" : "id";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.DriverSortParm = sortOrder == "DriverNameSort" ? "DriverNameSort_desc" : "DriverNameSort";
            ViewBag.CarrierSortParm = sortOrder == "CarrNameSort" ? "CarrNameSort_desc" : "CarrNameSort";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Inactive" : "Active";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;




            var vehicle = from s in db.Vehicles.Include(v => v.Carrier).Include(v => v.Driver)
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int numValue;
                bool parsed = Int32.TryParse(searchString, out numValue);

                if (parsed)
                {
                    vehicle = vehicle.Where(s => s.VehicleID == numValue);
                }
                else
                {
                    vehicle = vehicle.Where(s => s.VehicleCode.ToUpper().Contains(searchString.ToUpper()));
                }
            }
            switch (sortOrder)
            {
                case "id":
                    vehicle = vehicle.OrderBy(s => s.VehicleID);
                    break;
                case "id_desc":
                    vehicle = vehicle.OrderByDescending(s => s.VehicleID);
                    break;
                case "Name":
                    vehicle = vehicle.OrderBy(s => s.VehicleCode);
                    break;
                case "Name_desc":
                    vehicle = vehicle.OrderByDescending(s => s.VehicleCode);
                    break;
                case "DriverNameSort":
                    vehicle = vehicle.OrderBy(s => s.Driver.DriverName);
                    break;
                case "DriverNameSort_desc":
                    vehicle = vehicle.OrderByDescending(s => s.Driver.DriverName);
                    break;
                case "CarrNameSort":
                    vehicle = vehicle.OrderBy(s => s.Carrier.CarrierName);
                    break;
                case "CarrNameSort_desc":
                    vehicle = vehicle.OrderByDescending(s => s.Carrier.CarrierName);
                    break;
                case "Active":
                    vehicle = vehicle.OrderBy(s => s.Active);
                    break;
                case "Inactive":
                    vehicle = vehicle.OrderByDescending(s => s.Active);
                    break;
                default:
                    vehicle = vehicle.OrderByDescending(s => s.ModifiedDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            if (vehicle.Any())
            {
                SelectDriverIndex(vehicle.ToPagedList(pageNumber, pageSize).FirstOrDefault().CarrierID);
            }
            else
            {
                //List<SelectListItem> carrierData = new List<SelectListItem>();
                List<SelectListItem> driverData = new List<SelectListItem>();
                //ViewData["carrierName"] = carrierData;
                ViewData["DriverID"] = driverData;
            }
            SelectCarrier();
            return View(vehicle.ToPagedList(pageNumber, pageSize));
            //return View(customers.ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAutoComplete(string term)
        {
            List<String> _result;

            _result = db.Vehicles.Where(x => x.VehicleCode.ToString().StartsWith(term))
                              .Select(e => e.VehicleCode.ToString()).Distinct().ToList();

            return Json(_result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCarrier()
        {
            var carriers = from s in db.Carriers.OrderBy(s => s.CarrierName).Where(s => s.Active == true)
                           select s;

            List<SelectListItem> carrName = new List<SelectListItem>();
            foreach (var item in carriers)
            {
                carrName.Add(new SelectListItem { Text = item.CarrierName, Value = item.CarrierID.ToString() });
            }
            ViewData["CarrierID"] = carrName;
            return View();

        }
        public ActionResult SelectDriverIndex(int carriId)
        {
            List<SelectListItem> driverNames = new List<SelectListItem>();
            var drivers = from s in db.Drivers.Where(x => x.CarrierID == carriId && x.Active == true && x.LicenseEDate > DateTime.Now).OrderBy(s => s.DriverName)
                              select s;
                foreach (var item in drivers)
                {
                    driverNames.Add(new SelectListItem { Text = item.DriverName, Value = item.DriverID.ToString() });
                }
                ViewData["DriverID"] = driverNames;
                return View();
        }
        public ActionResult SelectDriver(string carriId)
        {
            int carrId;
            List<SelectListItem> driverNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(carriId))
            {
                carrId = Convert.ToInt32(carriId);
                var drivers = from s in db.Drivers.Where(x => x.CarrierID == carrId && x.Active == true && x.LicenseEDate > DateTime.Now).OrderBy(s => s.DriverName)
                               select s;
                foreach (var item in drivers)
                {
                    driverNames.Add(new SelectListItem { Text = item.DriverName, Value = item.DriverID.ToString() });
                }
            }
            return Json(driverNames);  
        }
        public ActionResult SelectDriverCNIC(int drvId)
        {
            List<SelectListItem> driverCNIC = new List<SelectListItem>();
            var drivers = from s in db.Drivers.Where(x => x.DriverID == drvId && x.Active == true && x.LicenseEDate > DateTime.Now)
                              select s;
                foreach (var item in drivers)
                {
                    driverCNIC.Add(new SelectListItem { Text = item.CNIC, Value = item.DriverID.ToString() });
                }
                return Json(driverCNIC);
        }
        // GET: Vehicles/Details/5
        public ActionResult UniqueCode(string code, int Id)
        {
            var vehicle = db.Vehicles.Where(x => x.VehicleCode == code);
            if (vehicle.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                if (Id != 0)
                {
                    var modifyResult = vehicle.Where(x => x.VehicleID == Id).ToList();
                    if (modifyResult.Count() == 0)
                    {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }

                }
                else
                {
                    return Json(false);
                }

            }

        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName");
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "DriverName");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehicleID,VehicleCode,LicenseNo,LicenseIDate,LicenseEDate,Active,LastActive,Remarks,CarrierID,DriverID")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {                
                    if (vehicle.VehicleID.Equals(0))
                    {
                        vehicle.CreatedDate = DateTime.Now;
                        vehicle.CreatedBy = User.Identity.Name;
                        vehicle.ModifiedDate = DateTime.Now;
                        vehicle.ModifiedBy = User.Identity.Name;
                        db.Vehicles.Add(vehicle);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        vehicle.ModifiedDate = DateTime.Now;
                        vehicle.ModifiedBy = User.Identity.Name;
                        db.Entry(vehicle).State = EntityState.Modified;
                        db.Entry(vehicle).Property(x => x.CreatedDate).IsModified = false;
                        db.Entry(vehicle).Property(x => x.CreatedBy).IsModified = false;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                    TempData["CustomError"] = "Vehicle Code is not Unique";
                    return RedirectToAction("Index");
                }

            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", vehicle.CarrierID);
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "DriverName", vehicle.DriverID);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", vehicle.CarrierID);
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "DriverName", vehicle.DriverID);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleID,VehicleCode,LicenseNo,LicenseIDate,LicenseEDate,Active,LastActive,Remarks,CarrierID,DriverID,ModifiedDate,CreatedDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", vehicle.CarrierID);
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "DriverName", vehicle.DriverID);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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
