using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Common.Models
{
    public interface IAuditInfo<TAuditUser>
        where TAuditUser : class
    {
        Guid? CreatedById { get; set; }

        TAuditUser CreatedBy { get; set; }

        Guid? ModifiedById { get; set; }

        TAuditUser ModifiedBy { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
