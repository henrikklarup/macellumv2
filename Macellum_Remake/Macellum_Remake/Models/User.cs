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
    
    public partial class User
    {
        public User()
        {
            this.Trips = new HashSet<Trip>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
    
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual Role Role { get; set; }
        public virtual Password Password { get; set; }
    }
}
