using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Data.Models.Entities
{
    public class ProductProcurement : BaseProdusctModel<Guid , ApplicationUser>
    {
         public string BuyersName { get; set; }

         public DateTime ProcurementsDay { get; set; }

         public string Comment { get; set; }

         public CommonMoneyAndProducts BlinBerry { get; set; }

         public Guid BlinBerryId { get; set; }

         public double KefirPrice { get; set; }
         public double EggsPrice { get; set; }
         public double VanilaPrice { get; set; }
         public double SaltPrice { get; set; }
         public double SugarPrice { get; set; }
         public double OilPrice { get; set; }
         public double SodaPrice { get; set; }
    }
}
