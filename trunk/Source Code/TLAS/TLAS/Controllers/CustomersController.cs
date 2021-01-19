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
    //[Authorize(Roles = "Supervisor")]
    [Authorize]
    public class CustomersController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();

        // GET: Customers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewBag.CurrentSort = sortOrder; // added new
            ViewBag.IdSortParm = sortOrder == "id" ? "id_desc" : "id";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            if (searchString != null)
            {
                page = 1;
            } 
            else
            { 
                searchString = currentFilter; 
            }
            ViewBag.CurrentFilter = searchString;




            var customers = from s in db.Customers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int numValue;
                bool parsed = Int32.TryParse(searchString, out numValue);

                if (parsed)
                {
                    customers = customers.Where(s => s.CustomerID==numValue);
                }
                else
                {
                    customers = customers.Where(s => s.CustomerName.ToUpper().Contains(searchString.ToUpper()));
                }               
            }
            switch (sortOrder)
            {
                case "id":
                    customers = customers.OrderBy(s => s.CustomerID);
                    break;
                case "id_desc":
                    customers = customers.OrderByDescending(s => s.CustomerID);
                    break;
                case "Name":
                    customers = customers.OrderBy(s => s.CustomerName);
                    break;
                case "Name_desc":
                    customers = customers.OrderByDescending(s => s.CustomerName);
                    break;
                case "Phone":
                    customers = customers.OrderBy(s => s.CustomerPhone);
                    break;
                case "Phone_desc":
                    customers = customers.OrderByDescending(s => s.CustomerPhone);
                    break;
                default:
                    customers = customers.OrderByDescending(s => s.ModifiedDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(customers.ToPagedList(pageNumber, pageSize));
            //return View(customers.ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAutoComplete(string term)
        {
            List<String> _result;

            _result = db.Customers.Where(x => x.CustomerName.ToString().StartsWith(term))
                              .Select(e => e.CustomerName.ToString()).Distinct().ToList();

            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UniqueName(string Name, int Id)
        {
            var customer = db.Customers.Where(x => x.CustomerName == Name);
            if (customer.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                if (Id != 0)
                {
                    var modifyResult = customer.Where(x => x.CustomerID == Id).ToList();
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

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,CustomerAddress,ContactName,CustomerPhone,CustomerMobile,CustomerEmail,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (customer.CustomerID.Equals(0))
                    {
                        customer.CreatedDate = DateTime.Now;
                        customer.CreatedBy = User.Identity.Name;
                        customer.ModifiedDate = DateTime.Now;
                        customer.ModifiedBy = User.Identity.Name;
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        customer.ModifiedDate = DateTime.Now;
                        customer.ModifiedBy = User.Identity.Name;
                        db.Entry(customer).State = EntityState.Modified;
                        db.Entry(customer).Property(x => x.CreatedDate).IsModified = false;
                        db.Entry(customer).Property(x => x.CreatedBy).IsModified = false;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["CustomError"] = "Customer Name is not Unique";
                    return RedirectToAction("Index");
                }
                
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,CustomerAddress,ContactName,CustomerPhone,CustomerMobile,CustomerEmail,Active,CreatedDate,ModifiedDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
            //return View(customer);
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
