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
    
    public partial class VehicleHistory
    {
        public int VehicleID { get; set; }
        public string VehicleCode { get; set; }
        public string LicenseNo { get; set; }
        public Nullable<System.DateTime> LicenseIDate { get; set; }
        public Nullable<System.DateTime> LicenseEDate { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> LastActive { get; set; }
        public string Remarks { get; set; }
        public int CarrierID { get; set; }
        public int DriverID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string HistoryType { get; set; }
    }
}
