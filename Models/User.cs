using System.Collections.Generic;


namespace ToDoList.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<ToDoCase> ToDoCases { get; set; }
        public List<DoneCase> DoneCases { get; set; }       
    }
}
