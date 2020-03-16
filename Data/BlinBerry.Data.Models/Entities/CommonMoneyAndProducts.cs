using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class CommonMoneyAndProducts : BaseProdusctModel<Guid, ApplicationUser>
    {
        public double? TotalCash { get; set; }

        public virtual ICollection<ProductProcurement> Procurements { get; set; }

        public virtual ICollection<SelesReport> Reports { get; set; }

        public virtual ICollection<Spending> Spending { get; set; }
    }
}
