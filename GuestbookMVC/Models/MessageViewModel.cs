namespace GuestbookMVC.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string AuthorLogin { get; set; }
    }
}
