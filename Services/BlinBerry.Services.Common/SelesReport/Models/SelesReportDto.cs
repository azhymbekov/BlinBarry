using System;
using System.Collections.Generic;
using System.Text;

namespace BlinBerry.Services.Common.SelesReport.Models
{
    public class SelesReportDto
    {
        public Guid Id { get; set; }

        public double CountOfKg { get; set; }

        public DateTime DayOfWeek { get; set; }

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
    }
}
