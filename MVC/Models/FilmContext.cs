using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace MVC
{
    // Чтобы подключиться к базе данных через Entity Framework, необходим контекст данных. 
    // Контекст данных представляет собой класс, производный от класса DbContext.
    //жадная загрузка
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public FilmContext(DbContextOptions<FilmContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                Genre g1 = new Genre { Name = "SciFi" };
                Genre g2 = new Genre { Name = "Action" };
                Genre g3 = new Genre { Name = "Adventure" };
                Genre g4 = new Genre { Name = "Comedy" };
                Genre g5 = new Genre { Name = "Family" };
                Genre g6 = new Genre { Name = "Romance" };
                Genre g7 = new Genre { Name = "Drama" };
                Genre g8 = new Genre { Name = "Thriller" };
                Genre g9 = new Genre { Name = "Horror" };
                Genre g10 = new Genre { Name = "Mystery" };
                Genre g11 = new Genre { Name = "Fantasy" };
                Genre g12 = new Genre { Name = "Crime" };
                Genre g13 = new Genre { Name = "Musical" };

                Genres?.Add(g1);
                Genres?.Add(g2);
                Genres?.Add(g3);
                Genres?.Add(g4);
                Genres?.Add(g5);
                Genres?.Add(g6);
                Genres?.Add(g7);
                Genres?.Add(g8);
                Genres?.Add(g9);
                Genres?.Add(g10);
                Genres?.Add(g11);
                Genres?.Add(g12);
                Genres?.Add(g13);

                Films?.Add(new Film { Name = "The Darkest Minds", Maker = "Jennifer Yuh", Genre = g1, Year = 2018, Poster = "/Images/The_Darkest_Minds.jpg", Description = "16-year-old Ruby and other teens, who survived a virus and gained special abilities, hide from the authorities, who deem them dangerous." });

                Films?.Add(new Film { Name = "The Amazing Spider-Man", Maker = "Mark Webb", Genre = g1, Year = 2012, Poster = "/Images/Spider-Man.jpg", Description = "Peter Parker, after being bitten by a genetically altered spider, becomes Spider-Man and faces the Lizard — his father’s former partner, turned into a monster. " });

                Films?.Add(new Film { Name = "Back to the Future", Maker = "Robert Zemeckis", Genre = g1, Year = 1985, Poster = "/Images/Back_to_the_Future.jpg", Description = "Young Marty McFly uses a time machine created by a mad scientist to travel to the past and alter the course of events." });

                Films?.Add(new Film { Name = "Harry Potter and the Deathly Hallows: Part 2", Maker = "David Yates", Genre = g1, Year = 2011, Poster = "/Images/Harry_Potter.jpg", Description = "Harry Potter and his friends battle Voldemort in the final part of the epic saga, striving to save the wizarding world." });

                Films?.Add(new Film { Name = "Home Alone", Maker = "Chris Columbus", Genre = g4, Year = 1990, Poster = "/Images/Home_Alone.jpg", Description = "Young Kevin is left home alone and has to fend off burglars trying to rob his house." });

                Films?.Add(new Film { Name = "The Notebook", Maker = "Nick Cassavetes", Genre = g6, Year = 2004, Poster = "/Images/The_Notebook.jpg", Description = "A love story spanning years, between Noah and Allie, whose relationship is tested by time and circumstance." });

                Films?.Add(new Film { Name = "Little Women", Maker = "Greta Gerwig", Genre = g7, Year = 2019, Poster = "/Images/Little_Women.jpg", Description = "The story of four sisters growing up in America during the Civil War, navigating their path to maturity and independence." });

                Films?.Add(new Film { Name = "Mothers Instinct", Maker = "Sergey Vinogradov", Genre = g8, Year = 2023, Poster = "/Images/Mothers_Instinct.jpg", Description = "A single mother named Olga discovers that her daughter has gone missing. She begins her own investigation to find the child and soon uncovers a dark secret." });

                Films?.Add(new Film { Name = "The Devil All the Time", Maker = "Antonio Campos", Genre = g8, Year = 2020, Poster = "/Images/The_Devil_All_The_Time.jpg", Description = "A young man is caught in the struggle between good and evil, growing up in rural Ohio during the 1960s, where violence and corruption define the people around him." });

                Films?.Add(new Film { Name = "Close to the Horizon", Maker = "Michael Karen", Genre = g7, Year = 2020, Poster = "/Images/Close_to_the_Horizon.jpg", Description = "A love story between two young people facing a life-changing illness. They must navigate the challenges of their emotions while dealing with the limitations of time and circumstance." });

                SaveChanges();
            }
        }

    }
}