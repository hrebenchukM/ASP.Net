using Microsoft.EntityFrameworkCore;

namespace GuestbookMVC.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }//тут нет DbSet  Register /Login так и понимает студия что доменная у нас лишь юзер

        public DbSet<Message> Messages { get; set; }
        
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}