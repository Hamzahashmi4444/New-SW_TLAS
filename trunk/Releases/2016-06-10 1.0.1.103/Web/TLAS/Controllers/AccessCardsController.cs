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
using TLAS.ViewModel;

namespace TLAS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccessCardsController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();

        //// GET: AccessCards
        //public ActionResult Index()
        //{
        //    var accessCards = db.AccessCards.Include(a => a.Bay);
        //    return View(accessCards.ToList());
        //}
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id" : "";
           // ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;




            var accessCard = from s in db.AccessCards
                             select new AccessCardViewModel { 
                                 CardID = s.CardID,
                                 BayID = s.BayID,
                                 Remarks = s.Remarks,
                                 Active = s.Active,
                                 IsAssigned = s.IsAssigned
                             };
            if (!String.IsNullOrEmpty(searchString))
            {
                int numValue;
                bool parsed = Int32.TryParse(searchString, out numValue);

                if (parsed)
                {
                    accessCard = accessCard.Where(s => s.CardID == numValue);
                }
                
            }
            switch (sortOrder)
            {
                case "id":
                    accessCard = accessCard.OrderBy(s => s.CardID);
                    break;
                default:
                    accessCard = accessCard.OrderByDescending(s => s.CardID);
                    break;
            }
            //SelectBay();
            ////////////////////////////////////////
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(accessCard.ToPagedList(pageNumber, pageSize));
            //return View(customers.ToList());
        }
        public ActionResult SelectBay()
        {
            List<SelectListItem> bayNames = new List<SelectListItem>();
            var bay = from x in db.Bays
                      select new {x.BayID};
            foreach (var item in bay)
            {
                bayNames.Add(new SelectListItem { Text = item.BayID.ToString(), Value = item.BayID.ToString() });
            }
            ViewData["BayID"] = bayNames;
            return View();
        }

        //public ActionResult UniqueName(string Name, int Id)
        //{
        //    var customer = db.Customers.Where(x => x.CustomerName == Name);
        //    if (customer.Count() == 0)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        if (Id != 0)
        //        {
        //            var modifyResult = customer.Where(x => x.CustomerID == Id).ToList();
        //            if (modifyResult.Count() == 0)
        //            {
        //                return Json(false);
        //            }
        //            else
        //            {
        //                return Json(true);
        //            }

        //        }
        //        else
        //        {
        //            return Json(false);
        //        }

        //    }

        //}

        // GET: AccessCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessCard accessCard = db.AccessCards.Find(id);
            if (accessCard == null)
            {
                return HttpNotFound();
            }
            return View(accessCard);
        }

        // GET: AccessCards/Create
        public ActionResult Create()
        {
            ViewBag.BayID = new SelectList(db.Bays, "BayID", "Remarks");
            return View();
        }

        // POST: AccessCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(AccessCard accessCard)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (accessCard.CardID.Equals(0))
                    {
                        db.AccessCards.Add(accessCard);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        accessCard.ModifiedDate = DateTime.Now;
                        db.AccessCards.Attach(accessCard);
                        db.Entry(accessCard).Property(x => x.BayID).IsModified = true;
                        db.Entry(accessCard).Property(x => x.Remarks).IsModified = true;
                        db.Entry(accessCard).Property(x => x.Active).IsModified = true;
                        db.Entry(accessCard).Property(x => x.IsAssigned).IsModified = true;
                        db.Entry(accessCard).Property(x => x.ModifiedDate).IsModified = true;
                        //db.Entry(accessCard).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["CustomError"] = "Card ID is not Unique";
                    return RedirectToAction("Index");
                }

            }

            ViewBag.BayID = new SelectList(db.Bays, "BayID", "Remarks", accessCard.BayID);
            return View(accessCard);
        }

        // GET: AccessCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessCard accessCard = db.AccessCards.Find(id);
            if (accessCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.BayID = new SelectList(db.Bays, "BayID", "Remarks", accessCard.BayID);
            return View(accessCard);
        }

        // POST: AccessCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CardID,BayID,Active,IsAssigned,LastActive,ModifiedDate,Remarks,CreatedDate,CardKey")] AccessCard accessCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accessCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BayID = new SelectList(db.Bays, "BayID", "Remarks", accessCard.BayID);
            return View(accessCard);
        }

        // GET: AccessCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessCard accessCard = db.AccessCards.Find(id);
            if (accessCard == null)
            {
                return HttpNotFound();
            }
            return View(accessCard);
        }

        // POST: AccessCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccessCard accessCard = db.AccessCards.Find(id);
            db.AccessCards.Remove(accessCard);
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
