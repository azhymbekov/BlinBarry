using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class SelesReport : BaseModel<Guid, ApplicationUser>
    {
        public double CountOfKg { get; set; }

        public DateTime DayOfWeek { get; set; }

        public double DefectiveKg { get; set; }       

    }
}
