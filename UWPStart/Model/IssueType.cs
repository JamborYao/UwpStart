//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSDNServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class IssueType
    {
        public IssueType()
        {
            this.Threads = new HashSet<Thread>();
        }
    
        public int Id { get; set; }
        public string IssueTypeName { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<int> CreateBy { get; set; }
    
        public virtual ICollection<Thread> Threads { get; set; }
    }
}
