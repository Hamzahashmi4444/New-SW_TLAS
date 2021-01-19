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
    [Authorize(Roles = "Admin")]
    public class BaysController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();

        // GET: Bays
        //public ActionResult Index()
        //{
        //    var bays = db.Bays.Include(b => b.Product);
        //    return View(bays.ToList());
        //}
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //return View(db.Products.ToList());
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;




            var bay = from s in db.Bays.Include(b => b.Product)
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int numValue;
                bool parsed = Int32.TryParse(searchString, out numValue);

                if (parsed)
                {
                    bay = bay.Where(s => s.BayID == numValue);
                }
                else
                {
                    bay = bay.Where(s => s.Product.ProductName.ToUpper().Contains(searchString.ToUpper()));
                }
            }
            switch (sortOrder)
            {
                case "id_desc":
                    bay = bay.OrderByDescending(s => s.BayID);
                    break;
                case "Name":
                    bay = bay.OrderBy(s => s.Product.ProductName);
                    break;
                case "Name_desc":
                    bay = bay.OrderByDescending(s => s.Product.ProductName);
                    break;
                default:
                    bay = bay.OrderBy(s => s.BayID);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(bay.ToPagedList(pageNumber, pageSize));
        }

        // GET: Bays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bay bay = db.Bays.Find(id);
            if (bay == null)
            {
                return HttpNotFound();
            }
            return View(bay);
        }

        // GET: Bays/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: Bays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BayID,Frequency,CurrQueue,Active,LastActive,Remarks,ModifiedDate,CreatedDate,ProductID")] Bay bay)
        {
            if (ModelState.IsValid)
            {
                db.Bays.Add(bay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", bay.ProductID);
            return View(bay);
        }

        // GET: Bays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bay bay = db.Bays.Find(id);
            if (bay == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", bay.ProductID);
            return View(bay);
        }

        // POST: Bays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BayID,Frequency,CurrQueue,Active,LastActive,Remarks,ModifiedDate,CreatedDate,ProductID")] Bay bay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", bay.ProductID);
            return View(bay);
        }

        // GET: Bays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bay bay = db.Bays.Find(id);
            if (bay == null)
            {
                return HttpNotFound();
            }
            return View(bay);
        }

        // POST: Bays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bay bay = db.Bays.Find(id);
            db.Bays.Remove(bay);
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
