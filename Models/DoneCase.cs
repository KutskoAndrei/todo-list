

namespace ToDoList.Models
{
    public class DoneCase
    {
        public int DoneCaseId { get; set; }
        public string Case { get; set; }
        public bool Selected { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
