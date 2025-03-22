using System.ComponentModel.DataAnnotations;

namespace GuestbookMVC.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string? Content { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

    }
}
