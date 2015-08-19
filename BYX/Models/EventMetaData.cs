using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BYX.Models
{
    [MetadataType(typeof(EventMetaData))]
    public partial class BYXEvent { }
    public class EventMetaData
    {
        [Display(Name="Start Time")]
        [Required(ErrorMessage="Start Time is required.")]
        public DateTime Event_StartDateTime { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Event_Name { get; set; }
    }

    [MetadataType(typeof(EventTypeMetaData))]
    public partial class EventType { }
    public class EventTypeMetaData
    {
        [Display(Name="Event Weight")]
        [Required(ErrorMessage = "Event Weight is required.")]
        public string EventType_Weight { get; set; }

        [Display(Name="Event Type")]
        [Required(ErrorMessage = "Event Type is required.")]
        public string EventType_Name { get; set; }
    }
}