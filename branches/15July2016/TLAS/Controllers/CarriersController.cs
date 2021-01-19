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
    public class CarriersController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();

        // GET: Carriers
        //public ActionResult Index()
        //{
        //    return View(db.Carriers.ToList());
        //}
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewBag.CurrentSort = sortOrder; // added new
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
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




            var carriers = from s in db.Carriers
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int numValue;
                bool parsed = Int32.TryParse(searchString, out numValue);

                if (parsed)
                {
                    carriers = carriers.Where(s => s.CarrierID == numValue);
                }
                else
                {
                    carriers = carriers.Where(s => s.CarrierName.ToUpper().Contains(searchString.ToUpper()));
                }
            }
            switch (sortOrder)
            {
                case "id":
                    carriers = carriers.OrderBy(s => s.CarrierID);
                    break;
                case "Name":
                    carriers = carriers.OrderBy(s => s.CarrierName);
                    break;
                case "Name_desc":
                    carriers = carriers.OrderByDescending(s => s.CarrierName);
                    break;
                case "Phone":
                    carriers = carriers.OrderBy(s => s.CarrierPhone);
                    break;
                case "Phone_desc":
                    carriers = carriers.OrderByDescending(s => s.CarrierPhone);
                    break;
                case "Active":
                    carriers = carriers.OrderBy(s => s.Active);
                    break;
                case "Inactive":
                    carriers = carriers.OrderByDescending(s => s.Active);
                    break;
                default:
                    carriers = carriers.OrderByDescending(s => s.CarrierID);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(carriers.ToPagedList(pageNumber, pageSize));
            //return View(customers.ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAutoComplete(string term)
        {
            List<String> _result;

            _result = db.Carriers.Where(x => x.CarrierName.ToString().StartsWith(term))
                              .Select(e => e.CarrierName.ToString()).Distinct().ToList();

            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        // GET: Carriers/Details/5
        public ActionResult UniqueName(string Name, int Id)
        {
            var carrier = db.Carriers.Where(x => x.CarrierName == Name);
            if (carrier.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                if(Id != 0)
                {
                    var modifyResult = carrier.Where(x => x.CarrierID == Id).ToList();
                        if(modifyResult.Count() == 0)
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

        // GET: Carriers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carriers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarrierID,CarrierName,CarrierAddress,ContactName,CarrierPhone,CarrierMobile,CarrierEmail,Active,LastActive")] Carrier carrier)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (carrier.CarrierID.Equals(0))
                    {
                        carrier.CreatedDate = DateTime.Now;
                        carrier.CreatedBy = User.Identity.Name;
                        carrier.ModifiedDate = DateTime.Now;
                        carrier.ModifiedBy = User.Identity.Name;
                        db.Carriers.Add(carrier);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        carrier.ModifiedDate = DateTime.Now;
                        carrier.ModifiedBy = User.Identity.Name;
                        db.Entry(carrier).State = EntityState.Modified;
                        db.Entry(carrier).Property(x => x.CreatedDate).IsModified = false;
                        db.Entry(carrier).Property(x => x.CreatedBy).IsModified = false;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["CustomError"] = "Carrier Name is not Unique";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return View(carrier);
        }

        // GET: Carriers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrier carrier = db.Carriers.Find(id);
            if (carrier == null)
            {
                return HttpNotFound();
            }
            return View(carrier);
        }

        // POST: Carriers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarrierID,CarrierName,CarrierAddress,ContactName,CarrierPhone,CarrierMobile,CarrierEmail,Active,ModifiedDate,LastActive,CreatedDate")] Carrier carrier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carrier);
        }

        // GET: Carriers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrier carrier = db.Carriers.Find(id);
            if (carrier == null)
            {
                return HttpNotFound();
            }
            return View(carrier);
        }

        // POST: Carriers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carrier carrier = db.Carriers.Find(id);
            db.Carriers.Remove(carrier);
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
