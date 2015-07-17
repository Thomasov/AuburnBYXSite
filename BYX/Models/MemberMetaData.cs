using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BYX.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Member { }
    public class MemberMetaData
    {
        [Display(Name="First Name")]
        [Required(ErrorMessage="First name is required lol.")]
        public string Member_FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string Member_LastName { get; set; }

        [Display(Name="Email Address")]
        [EmailAddress]
        public string Member_EmailAddress { get; set; }

        [Display(Name="Banner ID")]
        public long Member_BannerID { get; set; }
    }
}