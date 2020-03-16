using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class Spending : BaseProdusctModel<Guid, ApplicationUser>
    {
        public double Money { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public CommonMoneyAndProducts BlinBerry { get; set; }

        public Guid BlinBerryId { get; set; }
    }
}
