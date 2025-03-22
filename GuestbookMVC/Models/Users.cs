namespace GuestbookMVC.Models
{
    public class User
    {//нет валидации ибо для этой модели не будет вьюшек с формами
        public User()
        {
            this.Messages = new HashSet<Message>();
        }
        public int Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }//для усиления защиты

        public ICollection<Message>? Messages { get; set; }
    }
}