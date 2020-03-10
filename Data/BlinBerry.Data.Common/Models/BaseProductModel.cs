using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Common.Models
{
    public abstract class BaseProdusctModel<TKey, TAuditUser> : BaseModel<TKey, TAuditUser>
        where TAuditUser : class
    {
        public double Kefir { get; set; }
        public double Eggs { get; set; }
        public double Vanila { get; set; }
        public double Salt { get; set; }
        public double Sugar { get; set; }
        public double Oil { get; set; }
        public double Soda { get; set; }
    }
}
