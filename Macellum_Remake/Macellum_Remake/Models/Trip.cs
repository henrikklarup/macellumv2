//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Macellum_Remake.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trip
    {
        public int Id { get; set; }
        public bool Open { get; set; }
        public string FishList { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual User User { get; set; }
    }
}
