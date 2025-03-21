using System.ComponentModel.DataAnnotations;
namespace MVC
{
    public class Film
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]//атрибутивный подход клиентской  валиидацаии  аннотации span asp-validation-for
        public string? Name { get; set; }
        public Genre? Genre { get; set; }//собственно говоря навигационное свойство - ссілка на команду єтого игрока
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Maker { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Range(1900, 2025, ErrorMessage = "Недопустимый год")]//задаем диапазон
        public int Year { get; set; }
        public string? Poster { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        //Два свойства для внешнего ключа выходит как в DataBaseFirst только мы в CodeFirstd
        public int GenreId { get; set; }//внешний ключ теперь не создается по навигационному свойству автоматически . Здесь оно пригодится для комбобокса на странице а не для базы данных
    }
}