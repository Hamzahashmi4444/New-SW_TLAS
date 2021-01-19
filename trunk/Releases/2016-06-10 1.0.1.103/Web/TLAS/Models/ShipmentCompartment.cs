//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ShipmentCompartment
    {
        public int ID { get; set; }
        public Nullable<int> OrderedQuantity { get; set; }
        public Nullable<int> PlannedQuantity { get; set; }
        public Nullable<int> ActualBCUQuantity { get; set; }
        public string AccessCardKey { get; set; }
        public int BayID { get; set; }
        public string Product { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CompartmentID { get; set; }
        public int ShipmentID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string BayName { get; set; }
        public int CompartmentCode { get; set; }
        public int Capacity { get; set; }
    
        public virtual Bay Bay { get; set; }
        public virtual Compartment Compartment { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
