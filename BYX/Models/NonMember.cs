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
    
    public partial class NonMember
    {
        public NonMember()
        {
            this.Guardians = new HashSet<Guardian>();
            this.Potentials = new HashSet<Potential>();
        }
    
        public int NonMember_ID { get; set; }
        public string NonMember_FirstName { get; set; }
        public string NonMember_LastName { get; set; }
        public string NonMember_EmailAddress { get; set; }
        public bool Archived { get; set; }
    
        public virtual ICollection<Guardian> Guardians { get; set; }
        public virtual ICollection<Potential> Potentials { get; set; }
    }
}