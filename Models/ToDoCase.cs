
namespace ToDoList.Models
{
    public class ToDoCase
    {
        public int ToDoCaseId { get; set; }
        public string Case { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
