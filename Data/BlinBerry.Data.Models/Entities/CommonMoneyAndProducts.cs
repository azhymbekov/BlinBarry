using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class CommonMoneyAndProducts : BaseProdusctModel<Guid, ApplicationUser>
    {
        public double TotalCash { get; set; }
    }
}
