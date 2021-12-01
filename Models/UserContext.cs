using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoCase> ToDoCases { get; set; }
        public DbSet<DoneCase> DoneCases { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
