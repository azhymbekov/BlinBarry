
using System;
using System.ComponentModel.DataAnnotations;

namespace BlinBerry.Data.Common.Models
{
    public abstract class BaseModel<TKey, TAuditUser> : IAuditInfo<TAuditUser>
      where TAuditUser : class
    {
        [Key]
        public TKey Id { get; set; }

        public Guid? CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TAuditUser CreatedBy { get; set; }

        public virtual TAuditUser ModifiedBy { get; set; }
    }
}