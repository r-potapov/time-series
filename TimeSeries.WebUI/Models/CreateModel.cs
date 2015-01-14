using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeSeries.WebUI.Models
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }
        [Display(Name = "Email")]   
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}