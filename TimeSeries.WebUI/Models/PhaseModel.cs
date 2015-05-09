using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeSeries.Domain.Entities;

namespace TimeSeries.WebUI.Models
{
    public class PhaseModel
    {
        public TimeSerie TimeSerie { get; set; }
        [Required]
        [Display(Name = "T (тау)")]
        public int Tau { get; set; }
        [Display(Name = "Временной ряд")]
        public IEnumerable<TimeSerie> TimeSerieSource { get; set; }
    }
}