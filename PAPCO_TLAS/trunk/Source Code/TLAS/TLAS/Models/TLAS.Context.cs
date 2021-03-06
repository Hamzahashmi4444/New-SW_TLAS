﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TLAS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TLASPreEntities : DbContext
    {
        public TLASPreEntities()
            : base("name=TLASPreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccessCard> AccessCards { get; set; }
        public virtual DbSet<Bay> Bays { get; set; }
        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<Compartment> Compartments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatu> OrderStatus { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentCompartment> ShipmentCompartments { get; set; }
        public virtual DbSet<ShipmentStatu> ShipmentStatus { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<test> tests { get; set; }
        public virtual DbSet<Trailer> Trailers { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<WeighBridge> WeighBridges { get; set; }
        public virtual DbSet<AccessCardHistory> AccessCardHistories { get; set; }
        public virtual DbSet<CarrierHistory> CarrierHistories { get; set; }
        public virtual DbSet<CompartmentHistory> CompartmentHistories { get; set; }
        public virtual DbSet<CustomerHistory> CustomerHistories { get; set; }
        public virtual DbSet<DriverHistory> DriverHistories { get; set; }
        public virtual DbSet<OrderHistory> OrderHistories { get; set; }
        public virtual DbSet<ProductHistory> ProductHistories { get; set; }
        public virtual DbSet<ShipmentCompartmentHistory> ShipmentCompartmentHistories { get; set; }
        public virtual DbSet<ShipmentHistory> ShipmentHistories { get; set; }
        public virtual DbSet<TrailerHistory> TrailerHistories { get; set; }
        public virtual DbSet<VehicleHistory> VehicleHistories { get; set; }
        public virtual DbSet<WeighBridgeHistory> WeighBridgeHistories { get; set; }
    }
}
