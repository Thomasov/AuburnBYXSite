//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BYX.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Admin
    {
        public int Admin_ID { get; set; }
        public int Member_ID { get; set; }
        public int Role_ID { get; set; }
    
        public virtual AdminRole AdminRole { get; set; }
        public virtual Member Member { get; set; }
    }
}