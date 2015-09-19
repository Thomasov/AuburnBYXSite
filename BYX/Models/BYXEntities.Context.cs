﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class AuburnBYXDBEntities : DbContext
    {
        public AuburnBYXDBEntities()
            : base("name=AuburnBYXDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminRole> AdminRoles { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public virtual DbSet<BYXEvent> BYXEvents { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberType> MemberTypes { get; set; }
    
        public virtual ObjectResult<sp_EventAttendanceRecord_Result> sp_EventAttendanceRecord(Nullable<int> p_EventID)
        {
            var p_EventIDParameter = p_EventID.HasValue ?
                new ObjectParameter("p_EventID", p_EventID) :
                new ObjectParameter("p_EventID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_EventAttendanceRecord_Result>("sp_EventAttendanceRecord", p_EventIDParameter);
        }
    
        public virtual ObjectResult<sp_MemberAttendanceRecord_Result> sp_MemberAttendanceRecord(Nullable<int> p_Member_ID)
        {
            var p_Member_IDParameter = p_Member_ID.HasValue ?
                new ObjectParameter("p_Member_ID", p_Member_ID) :
                new ObjectParameter("p_Member_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_MemberAttendanceRecord_Result>("sp_MemberAttendanceRecord", p_Member_IDParameter);
        }
    }
}
