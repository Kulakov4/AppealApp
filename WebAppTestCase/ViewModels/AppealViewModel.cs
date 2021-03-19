using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppTestCase.Helpers;

namespace WebAppTestCase.ViewModels
{
    public class AppealViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email некорректный")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Обязательное поле")]
        [MyPhone(region: "RU", ErrorMessage = "Некорректный номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Сообщение")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public bool IsEditing { get; set; }

        public int Page { get; set; }
    }
}
