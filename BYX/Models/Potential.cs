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
    
    public partial class Potential
    {
        public int NonMember_ID { get; set; }
        public string SemesterSignedUp { get; set; }
        public Nullable<int> ReferredBy { get; set; }
        public bool Archived { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual NonMember NonMember { get; set; }
    }
}
