using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLAS.Models;
using PagedList;

namespace TLAS.Controllers
{
    [Authorize]
    public class DriversController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewBag.CurrentSort = sortOrder; // added new
            ViewBag.IdSortParm = sortOrder == "id" ? "id_desc" : "id";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
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


            var drivers = from s in db.Drivers.Include(d => d.Carrier)
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                drivers = drivers.Where(s => s.CNIC == searchString);
            }
            switch (sortOrder)
            {
                case "id":
                    drivers = drivers.OrderBy(s => s.DriverID);
                    break;
                case "id_desc":
                    drivers = drivers.OrderByDescending(s => s.DriverID);
                    break;
                case "Name":
                    drivers = drivers.OrderBy(s => s.DriverName);
                    break;
                case "Name_desc":
                    drivers = drivers.OrderByDescending(s => s.DriverName);
                    break;
                case "CarrNameSort":
                    drivers = drivers.OrderBy(s => s.Carrier.CarrierName);
                    break;
                case "CarrNameSort_desc":
                    drivers = drivers.OrderByDescending(s => s.Carrier.CarrierName);
                    break;
                case "Active":
                    drivers = drivers.OrderBy(s => s.Active);
                    break;
                case "Inactive":
                    drivers = drivers.OrderByDescending(s => s.Active);
                    break;
                default:
                    drivers = drivers.OrderByDescending(s => s.ModifiedDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            SelectCarrier();
            return View(drivers.ToPagedList(pageNumber, pageSize));
            //return View(customers.ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAutoComplete(string term)
        {
            List<String> _result;

            _result = db.Drivers.Where(x => x.CNIC.ToString().StartsWith(term))
                              .Select(e => e.CNIC.ToString()).Distinct().ToList();

            return Json(_result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCarrier()
        {
            var carriers = from s in db.Carriers.Where(s => s.Active == true).OrderBy(s => s.CarrierName)
                           select s;

            List<SelectListItem> carrName = new List<SelectListItem>();
            foreach (var item in carriers)
            {
                carrName.Add(new SelectListItem { Text = item.CarrierName, Value = item.CarrierID.ToString() });
            }
            ViewData["CarrierID"] = carrName;
            return View();

        }
        public ActionResult UniqueCNIC(string cnic, int Id)
        {
            var driver = db.Drivers.Where(x => x.CNIC == cnic);
            if (driver.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                if (Id != 0)
                {
                    var modifyResult = driver.Where(x => x.DriverID == Id).ToList();
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
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DriverID,DriverName,LicenseNo,LicenseIDate,LicenseEDate,CNIC,Mobile,Active,LastActive,Remarks,CarrierID")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (driver.DriverID.Equals(0))
                    {
                        driver.CreatedDate = DateTime.Now;
                        driver.CreatedBy = User.Identity.Name;
                        driver.ModifiedDate = DateTime.Now;
                        driver.ModifiedBy = User.Identity.Name;
                        db.Drivers.Add(driver);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        driver.ModifiedDate = DateTime.Now;
                        driver.ModifiedBy = User.Identity.Name;
                        db.Entry(driver).State = EntityState.Modified;
                        db.Entry(driver).Property(x => x.CreatedDate).IsModified = false;
                        db.Entry(driver).Property(x => x.CreatedBy).IsModified = false;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["CustomError"] = "Driver CNIC not Unique";
                    return RedirectToAction("Index");

                }

            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", driver.CarrierID);
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", driver.CarrierID);
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DriverID,DriverName,LicenseNo,LicenseIDate,LicenseEDate,CNIC,Mobile,Active,LastActive,Remarks,CarrierID,CreatedDate,ModifiedDate")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", driver.CarrierID);
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Driver driver = db.Drivers.Find(id);
            db.Drivers.Remove(driver);
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
