using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class SeleTransaction : BaseModel<Guid, ApplicationUser>
    {
        public double CountOfKg { get; set; }

        public DateTime Date { get; set; }

        public double DefectiveKg { get; set; }

        public double TotalProfit { get; set; }
    }
}
