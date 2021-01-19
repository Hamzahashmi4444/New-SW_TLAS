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
    public class OrdersController : Controller
    {
        private TLASPreEntities db = new TLASPreEntities();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            }
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id" : "";
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

            var orders = from s in db.Orders.Include(o => o.Carrier).Include(o => o.Customer).Include(o => o.Product)
                             .Include(o => o.OrderStatu).Include(o => o.Vehicle).Where(o => o.OrderStatusID != 3)
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.OrderCode == searchString);
            }
            switch (sortOrder)
            {
                case "id":
                    orders = orders.OrderBy(s => s.OrderID);
                    break;
                case "Name":
                    orders = orders.OrderBy(s => s.OrderCode);
                    break;
                case "Name_desc":
                    orders = orders.OrderByDescending(s => s.OrderCode);
                    break;
                default:
                    orders = orders.OrderByDescending(s => s.ModifiedDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            SelectProductType();
            SelectCustomer();
            SelectCarrier();


            try
            {
                SelectVehicleIndex(orders.FirstOrDefault().CarrierID);
            }
            catch
            {
                List<SelectListItem> vehicleData = new List<SelectListItem>();
                ViewData["VehicleID"] = vehicleData;
            }
            return View(orders.ToPagedList(pageNumber, pageSize));
        }
        public void SelectProductType()
        {
            var product = from s in db.Products.OrderBy(s => s.ProductName).Where(s => s.Active == true)
                            select s;
            List<SelectListItem> pro = new List<SelectListItem>();
            foreach (var item in product)
            {
                pro.Add(new SelectListItem { Text = item.ProductName, Value = item.ProductID.ToString() });
            }
            //if(pro.Find(id).Value != null)
            //{

            //}
            //pro.FirstOrDefault().Selected = true;
            ViewData["ProductID"] = pro;

        }
        public ActionResult SelectCustomer()
        {
            var customers = from s in db.Customers.OrderBy(s => s.CustomerName)
                            select s;

            List<SelectListItem> custName = new List<SelectListItem>();
            foreach (var item in customers)
            {
                custName.Add(new SelectListItem { Text = item.CustomerName, Value = item.CustomerID.ToString() });
            }

            ViewData["CustomerID"] = custName;
            return View();
        }
        public ActionResult SelectCarrier()
        {
            var Carriers = from s in db.Carriers.OrderBy(s => s.CarrierName).Where(s => s.Active == true)
                            select s;

            List<SelectListItem> carrName = new List<SelectListItem>();
            foreach (var item in Carriers)
            {
                carrName.Add(new SelectListItem { Text = item.CarrierName, Value = item.CarrierID.ToString() });
            }

            ViewData["CarrierID"] = carrName;
            return View();
        }
        public ActionResult SelectVehicleIndex(int? carriId)
        {
            List<SelectListItem> vehicleNames = new List<SelectListItem>();
            if (carriId != null)
            {
                var vehicles = from s in db.Vehicles.Where(x => x.CarrierID == carriId && x.Active == true)
                              select s;
                foreach (var item in vehicles)
                {
                    vehicleNames.Add(new SelectListItem { Text = item.VehicleCode, Value = item.VehicleID.ToString() });
                }
            }
            ViewData["VehicleID"] = vehicleNames;
            return View();
        }
        public ActionResult SelectVehicle(string carriId)
        {
            int carrId;
            List<SelectListItem> vehicleNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(carriId))
            {
                carrId = Convert.ToInt32(carriId);
                var drivers = from s in db.Vehicles.Where(x => x.CarrierID == carrId && x.Active == true)
                              select s;
                foreach (var item in drivers)
                {
                    vehicleNames.Add(new SelectListItem { Text = item.VehicleCode, Value = item.VehicleID.ToString() });
                }
            }
            return Json(vehicleNames);
        }
        public ActionResult DriverNameCNIC(string vehiId)
        {
            int vehhId;
            List<SelectListItem> nameCNIC = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(vehiId))
            {
                vehhId = Convert.ToInt32(vehiId);
                var vehicle = from x in db.Vehicles.Where(x => x.VehicleID == vehhId && x.Driver.Active == true)
                              select x;
                if(vehicle.Any())
                {
                    foreach (var item in vehicle)
                    {
                        nameCNIC.Add(new SelectListItem { Text = item.Driver.DriverName, Value = item.Driver.CNIC });
                    }
                }
                else
                {
                    nameCNIC.Add(new SelectListItem { Text = string.Empty, Value = string.Empty });
                }
                
            }
            else
            {
                nameCNIC.Add(new SelectListItem { Text = string.Empty, Value = string.Empty });
            }
            return Json(nameCNIC);
        }
        public ActionResult UniqueOrderCode(string Code, int Id)
        {
            var order = db.Orders.Where(x => x.OrderCode == Code);
            if (order.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                if (Id != 0)
                {
                    var modifyResult = order.Where(x => x.OrderID == Id).ToList();
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
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,OrderQty,RemainQty,CustomerID,ProductID,OrderCode,ModifiedDate,DeletedFlag,CreatedDate,CarrierID,VehicleID,OrderStatusID,OrderDeliveryDT")] Order order, FormCollection formCollection)
        {

            try { 
            if (ModelState.IsValid)
            {
                if (order.OrderID.Equals(0))
                {
                    var shipment = new Shipment()
                    {
                        ProductID = order.ProductID,
                        IsManual = false,
                        ModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        ShipmentStatusID = 1,
                        OrderID = order.OrderID,
                        CustomerName = formCollection["Customer.CustomerName"],
                        CarrierName = formCollection["Carrier.CarrierName"],
                        VehicleCode = formCollection["Vehicle.VehicleCode"],
                        DriverName = formCollection["Vehicle.Driver.DriverName"],
                        DriverCNIC = formCollection["Vehicle.Driver.CNIC"]
                    };
                    db.Orders.Add(order);
                    db.Shipments.Add(shipment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Shipment ship = db.Shipments.Where(x => x.OrderID == order.OrderID).FirstOrDefault<Shipment>();
                    ship.ModifiedDate = DateTime.Now;
                    ship.CustomerName = formCollection["Customer.CustomerName"];
                    ship.CarrierName = formCollection["Carrier.CarrierName"];
                    ship.VehicleCode = formCollection["Vehicle.VehicleCode"];
                    ship.DriverName = formCollection["Vehicle.Driver.DriverName"];
                    ship.DriverCNIC = formCollection["Vehicle.Driver.CNIC"];
                    ship.ProductID = order.ProductID;
                    db.Shipments.Attach(ship);
                    db.Entry(ship).Property(x => x.ModifiedDate).IsModified = true;
                    db.Entry(ship).Property(x => x.CustomerName).IsModified = true;
                    db.Entry(ship).Property(x => x.CarrierName).IsModified = true;
                    db.Entry(ship).Property(x => x.VehicleCode).IsModified = true;
                    db.Entry(ship).Property(x => x.DriverName).IsModified = true;
                    db.Entry(ship).Property(x => x.DriverCNIC).IsModified = true;
                    db.Entry(ship).Property(x => x.ProductID).IsModified = true;
                    db.Entry(ship).State = EntityState.Modified;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
                }
            catch
            {
                TempData["CustomError"] = "Order Code is not Unique";
                return RedirectToAction("Index");
            }

            ViewBag.CarrierID = new SelectList(db.Carriers, "CarrierID", "CarrierName", order.CarrierID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", order.CustomerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "ID", "Status", order.OrderStatusID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehicleCode", order.VehicleID);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
