using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeSeries.WebUI.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}