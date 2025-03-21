namespace MVC
{
    public class Genre
    {
        public Genre()
        {
            this.Films = new HashSet<Film>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Film>? Films { get; set; }
    }
}
