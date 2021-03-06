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
    
    public partial class Thread
    {
        public Thread()
        {
            this.UTs = new HashSet<UT>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<int> ForumID { get; set; }
        public Nullable<int> TeamID { get; set; }
        public Nullable<System.DateTime> ThreadCreateTime { get; set; }
        public Nullable<System.DateTime> FirstReplyTime { get; set; }
        public Nullable<int> RepliesNum { get; set; }
        public Nullable<int> TechCategoryID { get; set; }
        public Nullable<int> IssueTypeID { get; set; }
        public Nullable<double> IRT { get; set; }
        public Nullable<bool> IsReplied { get; set; }
        public Nullable<int> CSSAction { get; set; }
        public string Diffcult { get; set; }
        public string CustomerLookingFor { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<double> UT { get; set; }
        public string ThreadLink { get; set; }
        public string ThreadTitle { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsChanged { get; set; }
        public string Status { get; set; }
        public Nullable<int> Views { get; set; }
        public Nullable<int> SaveNumber { get; set; }
        public Nullable<int> SameNumber { get; set; }
    
        public virtual CSSAction CSSAction1 { get; set; }
        public virtual Engineer Engineer { get; set; }
        public virtual Engineer Engineer1 { get; set; }
        public virtual Forum Forum { get; set; }
        public virtual IssueType IssueType { get; set; }
        public virtual Team Team { get; set; }
        public virtual TechCategory TechCategory { get; set; }
        public virtual ICollection<UT> UTs { get; set; }
    }
}
