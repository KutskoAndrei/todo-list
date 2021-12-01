
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Укажите ваш Email")]        
        [EmailAddress(ErrorMessage ="Введите настоящий Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите ваш пароль")]
        [DataType(DataType.Password)]        
        public string Password { get; set; }
    }
}
