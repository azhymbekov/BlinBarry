using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlinBerry.Services.Common.SpendingService.Models
{
    public class SpendingDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Укажите cумму денег")]
        public double Money { get; set; }

        [Required(ErrorMessage = "Заполните причину расхода")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        public double Kefir { get; set; }
        public double Eggs { get; set; }
        public double Vanila { get; set; }
        public double Salt { get; set; }
        public double Sugar { get; set; }
        public double Oil { get; set; }
        public double Soda { get; set; }
    }
}
