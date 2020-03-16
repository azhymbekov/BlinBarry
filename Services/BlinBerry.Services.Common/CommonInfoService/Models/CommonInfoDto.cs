using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlinBerry.Services.Common.CommonInfoService.Models
{
    public class CommonInfoDto
    {
        public double Kefir { get; set; }
        public double Eggs { get; set; }
        public double Vanila { get; set; }
        public double Salt { get; set; }
        public double Sugar { get; set; }
        public double Oil { get; set; }
        public double Soda { get; set; }
        public double TotalCash { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? CreatedOn { get; set; }
    }
}
