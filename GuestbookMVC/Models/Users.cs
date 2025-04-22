using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GuestbookMVC.Models
{
    public class User
    {//нет валидации ибо для этой модели не будет вьюшек с формами
        public User()
        {
            this.Messages = new HashSet<Message>();
        }
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]//указываем регулярное выражение

        public string? Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Salt { get; set; }//для усиления защиты

        public ICollection<Message>? Messages { get; set; }
    }
}