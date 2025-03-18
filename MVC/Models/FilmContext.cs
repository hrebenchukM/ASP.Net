﻿using Microsoft.EntityFrameworkCore;

namespace MVC
{
    // Чтобы подключиться к базе данных через Entity Framework, необходим контекст данных. 
    // Контекст данных представляет собой класс, производный от класса DbContext.
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public FilmContext(DbContextOptions<FilmContext> options)
           : base(options)
        {
            if (Database.EnsureCreated())
            {
                Films?.Add(new Film { Name = "The Darkest Minds", Maker = "Jennifer Yuh", Genre = "SciFi/Action", Year = 2018, Poster = "/Images/The_Darkest_Minds.jpg", Description = "16-year-old Ruby and other teens, who survived a virus and gained special abilities, hide from the authorities, who deem them dangerous." });

                Films?.Add(new Film { Name = "The Amazing Spider-Man", Maker = "Mark Webb", Genre = "SciFi/Action", Year = 2012, Poster = "/Images/Spider-Man.jpg", Description = "Peter Parker, after being bitten by a genetically altered spider, becomes Spider-Man and faces the Lizard — his father’s former partner, turned into a monster. " });

                Films?.Add(new Film { Name = "Back to the Future", Maker = "Robert Zemeckis", Genre = "SciFi/Adventure", Year = 1985, Poster = "/Images/Back_to_the_Future.jpg", Description = "Young Marty McFly uses a time machine created by a mad scientist to travel to the past and alter the course of events." });

                Films?.Add(new Film { Name = "Harry Potter and the Deathly Hallows: Part 2", Maker = "David Yates", Genre = "Fantasy/Adventure", Year = 2011, Poster = "/Images/Harry_Potter.jpg", Description = "Harry Potter and his friends battle Voldemort in the final part of the epic saga, striving to save the wizarding world." });

                Films?.Add(new Film { Name = "Home Alone", Maker = "Chris Columbus", Genre = "Comedy/Family", Year = 1990, Poster = "/Images/Home_Alone.jpg", Description = "Young Kevin is left home alone and has to fend off burglars trying to rob his house." });

                Films?.Add(new Film { Name = "The Notebook", Maker = "Nick Cassavetes", Genre = "Romance/Drama", Year = 2004, Poster = "/Images/The_Notebook.jpg", Description = "A love story spanning years, between Noah and Allie, whose relationship is tested by time and circumstance." });

                Films?.Add(new Film { Name = "Little Women", Maker = "Greta Gerwig", Genre = "Drama", Year = 2019, Poster = "/Images/Little_Women.jpg", Description = "The story of four sisters growing up in America during the Civil War, navigating their path to maturity and independence." });


                SaveChanges();
            }
        }
    }
}