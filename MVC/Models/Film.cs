
namespace MVC
{
    public class Film
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Genre? Genre { get; set; }//собственно говоря навигационное свойство - ссілка на команду єтого игрока
        public string? Maker { get; set; }
        public int Year { get; set; }
        public string? Poster { get; set; }
        public string? Description { get; set; }
        //Два свойства для внешнего ключа выходит как в DataBaseFirst только мы в CodeFirstd
        public int GenreId { get; set; }//внешний ключ теперь не создается по навигационному свойству автоматически . Здесь оно пригодится для комбобокса на странице а не для базы данных

    }
}