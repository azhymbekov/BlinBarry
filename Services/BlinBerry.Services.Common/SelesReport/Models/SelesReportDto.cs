﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlinBerry.Services.Common.SelesReport.Models
{
    public class SelesReportDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите количество килограммов")]
        public double CountOfKg { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DayOfWeek { get; set; }

        [Required(ErrorMessage = "Введите количество браков")]
        public double DefectiveKg { get; set; }

        public double TotalKg
        {
            get
            {
                return DefectiveKg + CountOfKg;
            }
            set
            {

            } 
        }
        public double Cash { get; set; }
    }
}
