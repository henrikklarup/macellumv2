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
    
    public partial class Fisk
    {
        public int Id { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Sort { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Arter Arter { get; set; }
        public virtual Havn Havn { get; set; }
    }
}
