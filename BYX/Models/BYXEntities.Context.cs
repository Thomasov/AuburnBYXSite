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
    
    public partial class BYXEntities : DbContext
    {
        public BYXEntities()
            : base("name=BYXEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public virtual DbSet<BYXEvent> BYXEvents { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberType> MemberTypes { get; set; }
    }
}
