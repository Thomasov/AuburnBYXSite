using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BYX.Models
{
    public partial class NonMember
    {
        [Display(Name="Name")]
        public string FullName
        {
            get { return this.NonMember_FirstName + " " + this.NonMember_LastName; }
            set {
                string[] names = value.Split(' ');
                this.NonMember_FirstName = names[0];
                this.NonMember_LastName = names[names.Length - 1];
            }
        }


        
    }
}