using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlinBerry.Services.Common.SelesReport.Models;
using GlobalContants;

namespace BlinBerry.Services.Common.ProcurementService.Models
{
    public class ProcurementDto
    {
        public Guid Id { get; set; }
        public double Kefir { get; set; }
        public double Eggs { get; set; }
        public double Vanila { get; set; }
        public double Salt { get; set; }
        public double Sugar { get; set; }
        public double Oil { get; set; }
        public double Soda { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string BuyersName { get; set; }


        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ProcurementsDay { get; set; }

        public string Comment { get; set; }


        public double KefirPrice { get; set; }
        public double EggsPrice { get; set; }
        public double VanilaPrice { get; set; }
        public double SaltPrice { get; set; }
        public double SugarPrice { get; set; }
        public double OilPrice { get; set; }
        public double SodaPrice { get; set; }

        private double totalsum;
        public double TotalSum
        {
            get
            {
                return totalsum = KefirPrice + OilPrice + SodaPrice
                       + EggsPrice + SaltPrice + SugarPrice + VanilaPrice;
            }
            set => totalsum = KefirPrice + OilPrice + SodaPrice
                              + EggsPrice + SaltPrice + SugarPrice + VanilaPrice;
        }
    }
}
