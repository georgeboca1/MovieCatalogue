using System;
using System.Collections.Generic;
using System.Linq;


namespace MovieModels
{
    public enum GenreType
    {
        None = 0,
        Action = 1,
        Adventure = 2,
        Animation = 3,
        Biography = 4,
        Comedy = 5,
        Crime = 6,
        Drama = 7,
        Family = 8
    }

    public class Movie
    {
        private Guid Uuid;
        private Character Director;
        private List<Character> actors = new List<Character>();
        // -1 unrated; min 1 - max 10; example: rating 5 = 2.5 stars
        private int Rating { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Review { get; set; }
        public string ImagePath { get; set; }
        public int watched { get; set; } = 0; // 0 - not watched, 1 - watched
        private GenreType Genre { get; set; }
        private int year { get; set; }

        public Movie()
        {
            this.Uuid = Guid.NewGuid();
            this.Name = "Undefined";
            this.Description = "Undefined";
            this.Rating = -1;
            this.Review = string.Empty;
            this.Genre = GenreType.None;
            this.Director = new Character("null", DateTime.Today.Year);
            this.ImagePath = string.Empty;

        }

        public Movie(string name, string description, string imagePath)
        {
            this.Uuid = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Rating = -1;
            this.Review = string.Empty;
            this.Genre = GenreType.None;
            this.Director = new Character("null null", DateTime.Today.Year);
            this.ImagePath = imagePath;
        }

        public Movie(Guid uuid, string name, string description, string imagePath)
        {
            this.Uuid = uuid;
            this.Name = name;
            this.Description = description;
            this.Rating = -1;
            this.Genre = GenreType.None;
            this.Review = string.Empty;
            this.Director = new Character("null null", DateTime.Today.Year);
            this.ImagePath = imagePath;
        }

        public Movie(string fileInfo)
        {
            string[] data = fileInfo.Split(';');
            this.Uuid = Guid.Parse(data[0]);
            this.Name = data[1];
            this.Description = data[2];
            this.Rating = Int32.Parse(data[3]);
            this.Review = data[4];
            this.Director = new Character("null null", DateTime.Today.Year);
            this.Genre = (GenreType)Enum.Parse(typeof(GenreType),data[7]);
            this.ImagePath = data[8];
            this.watched = Convert.ToInt32(data[9]);
            this.year = Int32.Parse(data[10]);
        }

        public Guid GetUUID()
        {
            return this.Uuid;
        }

        public void AddRating(int rating)
        {
            if (rating < 0 || rating > 10)
            {
                throw new ArgumentOutOfRangeException("Rating must be between 0 and 10");
            }
            this.Rating = rating;
        }

        public void AddReview(string review)
        {
            this.Review = review;
        }

        public void AddYear(int year)
        {
            this.year = year;
        }

        public int GetYear(int year)
        {
            return year;
        }

        public void AddDirector(Character director)
        {
            this.Director = director;
        }
        
        public void AddActor(Character actor)
        {
            this.actors.Add(actor);
        }

        public void RemoveActor(Character actor)
        {
            this.actors.Remove(actor);
        }

        public void AddGenre(string genre)

        {
            this.Genre = (GenreType)Enum.Parse(typeof(GenreType),genre);
        }

        public Character GetDirector()
        {
            return Director;
        }

        public List<Character> GetActors()
        {
            return actors;
        }

        public int GetRating()
        {
            return Rating;
        }

        public string GetGenreString()
        {
            return Genre.ToString();
        }

        public string MovieInfo()
        {
            return $"UUID: {this.Uuid}\nNume: {this.Name}\nDescriere: {this.Description}\nGenre: {this.Genre.ToString()}\nRating: {(this.Rating == -1 ? "Fara rating" : Convert.ToString((float)Rating / 2))}\nReview: {(this.Review == string.Empty ? "Fara review" : this.Review)}\nDirector: {this.Director.FullName}\nActori:{string.Join(",",this.actors.ToArray().Select(a => a.FullName))}\n";
        }

        public string DatabaseInfo()
        {
            return $"{this.Uuid};{this.Name};{this.Description};{this.Rating};{this.Review};{this.Director?.GetUUID()};{string.Join("|",this.actors.ToArray().Select(a => a?.GetUUID()))};{this.Genre.ToString()};{this.ImagePath};{this.watched};{this.year}";
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
