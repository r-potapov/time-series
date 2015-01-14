using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TimeSeries.Domain.Entities
{
    public class TimeSerie
    {
        [HiddenInput(DisplayValue = false)]
        public int TimeSerieId { get; set; }
        [Display(Name = "Данные временного ряда")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Не указаны данные временного ряда")]
       // [ValidVectorDataAttribute(ErrorMessage = "Неверно указаны данные временного ряда")]
        public string VectorData { get; set; }
        public string User_Id { get; set; }
        public virtual AppUser User { get; set; }
    }
}
