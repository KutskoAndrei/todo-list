
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    //для работы с добавлением в базу нового дела
    public class AddCaseModel
    {
        [Required(ErrorMessage ="Это вы уже сделали ^_^")]
        [DataType(DataType.Text)]
        [StringLength(250, MinimumLength =2, ErrorMessage ="Длинна строки от 2 до 250 символов")]
        public string modelCase { get; set; }
    }
}
